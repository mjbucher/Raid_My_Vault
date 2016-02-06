using UnityEngine;
using System.Collections;
using Motive.Unity.Maps;
using Motive.AR.LocationServices;
using Motive.AR.Scripting;
using Motive.Unity.Utilities;
using System;
using System.Collections.Generic;
using Motive.Core.Utilities;
using System.Linq;
using Motive.Core.Diagnostics;
using Motive.UI;
using Motive.Unity.UI;
using Motive.Core.Models;

/// <summary>
/// The map controller interprets user input and sends it to the MapView and
/// manages the annotations on the map. This is a very quick & dirty implementation
/// that could use a lot of work!
/// </summary>
public class MapController : SingletonComponent<MapController>, IMapViewDelegate {

    public AnnotationGameObject Wheat;
    public AnnotationGameObject Ore;
    public AnnotationGameObject Wool;
    public AnnotationGameObject Wood;
    public AnnotationGameObject Brick;
    public AnnotationGameObject Task;
    public AnnotationGameObject TradePost;

    public AnnotationGameObject User;

    public CustomImageAnnotation CustomImageAnnotation;

    public MapView MapView;

    public MapInput MapInput;

    public double ZoomLevel = 15;
	public double PanSpeed = 0.1;

    public bool UseTaskImageForAnnotation = false;

    private bool m_firstUpdateComplete;
    private double m_startMapZoom;
    private Coordinates m_startCoordinates;

    private SetDictionary<string, MapAnnotation> m_taskAnnotations;
    private SetDictionary<string, MapAnnotation> m_markerAnnotations;
    private Dictionary<string, MapAnnotation> m_searchAnnotations;

    private UserAnnotation m_userAnnotation;
	private Coordinates m_panToCoordinates;
	private Action m_onPanComplete;

    private Dictionary<string, MediaElement> m_locationTypeMarkers;
    private Dictionary<string, MediaElement> m_storyTagMarkers;
    private Dictionary<string, Texture> m_cachedAnnotationTextures;

    public MapAnnotation SelectedAnnotation { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        m_taskAnnotations = new SetDictionary<string, MapAnnotation>();
        m_searchAnnotations = new Dictionary<string, MapAnnotation>();
        m_markerAnnotations = new SetDictionary<string, MapAnnotation>();
        m_locationTypeMarkers = new Dictionary<string, MediaElement>();
        m_storyTagMarkers = new Dictionary<string, MediaElement>();
        m_cachedAnnotationTextures = new Dictionary<string, Texture>();
    }


    private void HandleLocationsUpdated(object sender, System.EventArgs e)
    {
        var oldSearchAnns = m_searchAnnotations;
        m_searchAnnotations = new Dictionary<string, MapAnnotation>();

        ThreadHelper.Instance.CallOnMainThread(() =>
        {
            foreach (var loc in LocationCache.Instance.Locations)
            {
                // We're only interested in locations with story tags
                if (loc.StoryTags != null && loc.StoryTags.Length > 0)
                {
                    if (oldSearchAnns.ContainsKey(loc.Id))
                    {
                        // We already have this annotation,
                        // no need to re-add
                        var ann = oldSearchAnns[loc.Id];
                        oldSearchAnns.Remove(loc.Id);
                        m_searchAnnotations[loc.Id] = ann;
                    }
                    else
                    {
                        var ann = new MapAnnotation(loc);
                        m_searchAnnotations[loc.Id] = ann;

                        // When resolving markers, check location types first,
                        // then story tags.
                        if (loc.LocationTypes != null)
                        {
                            foreach (var t in loc.LocationTypes)
                            {
                                MediaElement marker = null;

                                if (m_locationTypeMarkers.TryGetValue(t, out marker))
                                {
                                    ann.Marker = marker;
                                    break;
                                }
                            }
                        }

                        if (ann.Marker == null && loc.StoryTags != null)
                        {
                            foreach (var s in loc.StoryTags)
                            {
                                MediaElement marker = null;

                                if (m_storyTagMarkers.TryGetValue(s, out marker))
                                {
                                    ann.Marker = marker;
                                    break;
                                }
                            }
                        }

                        MapView.AddAnnotation(ann);
                    }
                }
            }

            // Any that are left in the old dictionary can be removed
            foreach (var ann in oldSearchAnns.Values)
            {
                MapView.RemoveAnnotation(ann);
            }
        });
    }

    protected override void Start()
    {
        MapView.Delegate = this;
        LocationCacheDriver.Instance.Updated += HandleLocationsUpdated;
    }

    Vector2 m_translateTarget;
    Vector2 m_currTranslate;

	// Update is called once per frame
	void Update () {
        if (!m_firstUpdateComplete)
        {
            if (SystemPositionService.Instance.HasLocationData)
            {
                m_userAnnotation = new UserAnnotation();
                m_userAnnotation.Coordinates = SystemPositionService.Instance.Position;

                MapView.AddAnnotation(m_userAnnotation);

                var startCoords = m_startCoordinates ?? SystemPositionService.Instance.Position;
                MapView.SetRegion(startCoords, ZoomLevel);
                m_startMapZoom = ZoomLevel;

                m_firstUpdateComplete = true;
            }
        }
        else
        {
            // The map view will not necessarily re-check the coordinates unless the
            // told directly.
            m_userAnnotation.UpdateCoordinates(SystemPositionService.Instance.Position, Time.deltaTime);
            MapView.UpdateAnnotation(m_userAnnotation);

            if (MapInput.IsTranslating)
            {
				m_panToCoordinates = null;

                SelectAnnotation(null);

				if (MapInput.Translation.sqrMagnitude > 0)
                {
					MapView.Translate(-MapInput.Translation.x, MapInput.Translation.y);
                }
            }
            else
            {
                m_currTranslate = Vector2.zero;

				if (m_panToCoordinates != null)
				{
					var zoom = MapView.Zoom == 0 ? ZoomLevel : MapView.Zoom;
					
					var coordinates = MapView.CenterCoordinates.Approach(m_panToCoordinates, PanSpeed, Time.deltaTime);
					
					MapView.SetRegion(coordinates, zoom);

					if (coordinates.GetDistanceFrom(m_panToCoordinates) < 0.5)
					{
						m_panToCoordinates = null;

						if (m_onPanComplete != null)
						{
							m_onPanComplete();
						}
					}
				}				
			}

            if (MapInput.IsPinching)
            {
                MapView.SetRegion(MapView.CenterCoordinates, MapView.Zoom + MapInput.ZoomDelta);
            }
        }
	}
    public void SetAnnotationMarkerCatalog(Catalog<AnnotationMarker> catalog)
    {
        foreach (var am in catalog)
        {
            if (am.Marker != null)
            {
                if (am.LocationTypes != null)
                {
                    foreach (var t in am.LocationTypes)
                    {
                        m_locationTypeMarkers[t] = am.Marker;
                    }
                }

                if (am.StoryTags != null)
                {
                    foreach (var s in am.StoryTags)
                    {
                        m_storyTagMarkers[s] = am.Marker;
                    }
                }
            }
        }
    }

    public void AddTaskLocations(LocationTask task)
    {
        if (task.Locations != null)
        {
            foreach (var l in task.Locations)
            {
                var ann = new MapAnnotation(l);

                ann.LocationTask = task;

                m_taskAnnotations.Add(task.Id, ann);

                MapView.AddAnnotation(ann);
            }
        }
    }

    internal void RemoveTaskLocations(LocationTask task)
    {
        var annotations = m_taskAnnotations[task.Id];
        m_taskAnnotations.RemoveAll(task.Id);

        if (annotations != null)
        {
            foreach (var ann in annotations)
            {
                MapView.RemoveAnnotation(ann);
            }
        }
    }

    internal void AddLocationMarker(LocationMarker marker)
    {
        if (marker.Locations != null)
        {
            foreach (var l in marker.Locations)
            {
                var ann = new MapAnnotation(l);

                ann.Marker = marker.Marker;

                m_markerAnnotations.Add(marker.Id, ann);

                MapView.AddAnnotation(ann);
            }
        }
    }

    internal void RemoveLocationMarker(LocationMarker marker)
    {
        var annotations = m_markerAnnotations[marker.Id];
        m_markerAnnotations.RemoveAll(marker.Id);

        if (annotations != null)
        {
            foreach (var ann in annotations)
            {
                MapView.RemoveAnnotation(ann);
            }
        }
    }
    internal void SelectTaskAnnotation(LocationTask task)
    {
        var annotations = m_taskAnnotations[task.Id];

        if (annotations != null)
        {
            var ann = annotations.FirstOrDefault();

            if (ann != null)
            {
                SelectAnnotation(ann);
            }
        }
    }

    public void CenterMap()
    {
        CenterMap(null);
    }

    public void CenterMap(Coordinates coordinates, Action onPanComplete = null)
    {
		MapInput.CancelPan();

        if (coordinates == null)
        {
            coordinates = SystemPositionService.Instance.Position;
        }

        if (MapView != null)
		{
			m_panToCoordinates = coordinates;
			m_onPanComplete = onPanComplete;
        }
        else
        {
            m_startCoordinates = coordinates;
        }
    }

    #region IMapViewDelegate
    public System.Collections.Generic.IEnumerable<IAnnotation> GetAnnotationsForCluster(MapView sender, AnnotationCluster cluster)
    {
        throw new System.NotImplementedException();
    }

    public ClusterId GetClusterIdForAnnotation(MapView sender, IAnnotation annotation, Motive.Core.Utilities.DoubleVector2 offset)
    {
        throw new System.NotImplementedException();
    }

    AnnotationGameObject CreateMarkerAnnotation(MediaElement marker)
    {
        CustomImageAnnotation annotationObj = Instantiate(CustomImageAnnotation);

        if (marker.MediaUrl != null)
        {
            if (marker.Layout != null)
            {
                LayoutHelper.Apply(annotationObj.transform, marker.Layout);
            }

            StartCoroutine(ImageLoader.LoadImage(marker.MediaUrl, annotationObj.gameObject, m_cachedAnnotationTextures));
        }

        return annotationObj;
    }
    public GameObject GetObjectForAnnotation(MapView sender, IAnnotation annotation)
    {
        if (annotation is UserAnnotation)
        {
            return Instantiate<AnnotationGameObject>(User).gameObject;
        }

        AnnotationGameObject annotationObj = null;

        var mapAnnotation = annotation as MapAnnotation;

        if (mapAnnotation.LocationTask != null)
        {
            if (mapAnnotation.LocationTask.Marker != null)
            {
                annotationObj = CreateMarkerAnnotation(mapAnnotation.LocationTask.Marker);
            }
            else if (UseTaskImageForAnnotation && mapAnnotation.LocationTask.ImageUrl != null)
            {
                var imgAnnot = Instantiate<CustomImageAnnotation>(CustomImageAnnotation);

                annotationObj = imgAnnot;

                annotationObj.gameObject.SetActive(false);

                StartCoroutine(ImageLoader.LoadImage(mapAnnotation.LocationTask.ImageUrl, (tex) =>
                {
                    annotationObj.gameObject.SetActive(true);

                    imgAnnot.gameObject.GetComponentInChildren<Renderer>().material.mainTexture = tex;
                }));
            }
            else
            {
                annotationObj = Instantiate<AnnotationGameObject>(Task);
            }
        }
        else if (mapAnnotation.Marker != null)
        {
            annotationObj = CreateMarkerAnnotation(mapAnnotation.Marker);
        }
        else
        {
            if (mapAnnotation.Location.HasStoryTagsOr("wheat"))
            {
                annotationObj = Instantiate<AnnotationGameObject>(Wheat);
            }
            else if (mapAnnotation.Location.HasStoryTagsOr("ore"))
            {
                annotationObj = Instantiate<AnnotationGameObject>(Ore);
            }
            else if (mapAnnotation.Location.HasStoryTagsOr("wood"))
            {
                annotationObj = Instantiate<AnnotationGameObject>(Wood);
            }
            else if (mapAnnotation.Location.HasStoryTagsOr("brick"))
            {
                annotationObj = Instantiate<AnnotationGameObject>(Brick);
            }
            else if (mapAnnotation.Location.HasStoryTagsOr("wool"))
            {
                annotationObj = Instantiate<AnnotationGameObject>(Wool);
            }
            else
            {
                annotationObj = Instantiate<AnnotationGameObject>(TradePost);
            }
        }

        if (annotationObj != null)
        {
            annotationObj.Annotation = mapAnnotation;
        }

        return annotationObj.gameObject;
    }

    public Vector3 GetPositionForAnnotation(MapView sender, IAnnotation annotation, Motive.Core.Utilities.DoubleVector2 offset)
    {
        // The map view needs to know where the annotation lives on the texture.
        // We used a plane for the texture, which has dimensions of 10x10.
        // The offset in the call tells us where the annotation sits on the texture
        // surface, from extents 0,0 to 1,1. We need to convert these values to
        // a location on the plane itself.
        float dy = 0.1f;

        Vector3 pos = new
            Vector3((float)offset.X * 10.0f - 5f, MapView.MapTexture.transform.position.y + dy, (float)offset.Y * 10.0f - 5f);

        return pos;
    }

    public void RecycleAnnotationObject(MapView sender, GameObject annotationObject)
    {
        Destroy(annotationObject);
    }

    public bool ShouldClusterAnnotation(MapView sender, IAnnotation annotation)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    internal void SelectAnnotation(MapAnnotation mapAnnotation)
    {
        SelectedAnnotation = mapAnnotation;

        if (mapAnnotation != null)
        {
            PanelManager.Instance.Show<SelectedLocationPanel>(mapAnnotation);
        }
        else
        {
            PanelManager.Instance.HideAll();
        }

        if (mapAnnotation != null)
        {
            CenterMap(mapAnnotation.Coordinates);
        }
    }
}
