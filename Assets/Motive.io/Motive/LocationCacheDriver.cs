using UnityEngine;
using System.Collections;

using Motive.AR.LocationServices;
using Motive.Core.Scripting;
using Motive.Core.Diagnostics;
using Motive.AR.Scripting;
using System.Collections.Generic;
using Motive.Core.Models;
using System;
using Motive.Core.Utilities;
//using Motive.Core.Diagnostics;

public enum LocationCacheSource
{
    Foursquare,
    Catalog,
    Both
}

/// <summary>
/// This class keeps the local location cache populated with nearby search results. This
/// keeps our Foursquare (or other search provider) traffic to a minimum.
/// </summary>
public class LocationCacheDriver : SingletonComponent<LocationCacheDriver> {
    public int SearchMoveDistance = 100;
    public int SearchRange = 1000;
    
    private Coordinates m_lastCoordinates;
    private ILocationSearchProvider m_searchProvider;
    private ILocationSearch m_search;

    // Add some searches specifically for the story tags we're interested in
    // to augment the location cache.
    private List<ILocationSearch> m_storyTagSearches;

    public LocationCacheSource Source = LocationCacheSource.Foursquare;
    public string LocationCatalogName;

    private Catalog<Location> m_locationCatalog;

	Motive.Core.Diagnostics.Logger m_logger;
    bool m_initialized;

    public bool CacheReady { get; private set; }

    public event EventHandler Updated;

    protected override void Awake()
    {
        base.Awake();

        m_storyTagSearches = new List<ILocationSearch>();
    }

    public void Initialize()
    {
        if (m_initialized) return;

        m_initialized = true;

		m_logger = new Motive.Core.Diagnostics.Logger(this);

        m_logger.Debug("Initialize");

        if (Source == LocationCacheSource.Foursquare)
        {
            SetUpFoursquare();
        }
        else if (Source == LocationCacheSource.Catalog ||
                 Source == LocationCacheSource.Both)
        {
            WebServices.Instance.LoadCatalog<Location>(LocationCatalogName, (catalog) =>
            {
                m_locationCatalog = catalog;

                if (Source == LocationCacheSource.Both)
                {
                    SetUpFoursquare();
                }
                else
                {
                    LocationCache.Instance.SetCachedLocations(catalog);
                    CacheReady = true;
                }
            });
        }
    }

    void SetUpFoursquare()
    {
        m_searchProvider = FoursquareService.Instance.GetSearchProvider(FoursquareSearchIntent.Browse);

        SystemPositionService.Instance.PositionUpdated += HandleSystemPositionUpdated;

        if (SystemPositionService.Instance.HasLocationData)
        {
            HandleSystemPositionUpdated(SystemPositionService.Instance.Position);
        }
    }

    void DoStoryTagSearch(Action onDone, params string[] storyTags)
    {
        var search = m_searchProvider.CreateSearch();

        lock (m_storyTagSearches)
        {
            m_storyTagSearches.Add(search);
        }

        search.SearchStoryTags(
            SystemPositionService.Instance.Position,
            storyTags,
            1000,
            5 * storyTags.Length,
            () =>
            {
                if (search.Locations != null)
                {
                    LocationCache.Instance.AddLocationsToCache(search.Locations);
                }

                lock (m_storyTagSearches)
                {
                    m_storyTagSearches.Remove(search);
                    if (m_storyTagSearches.Count == 0)
                    {
                        onDone();
                    }
                }
            });
    }

    void SearchCompleted()
    {
        m_logger.Debug("Completed search");

        if (m_locationCatalog != null)
        {
            LocationCache.Instance.SetCachedLocations(m_locationCatalog);
            LocationCache.Instance.AddLocationsToCache(m_search.Locations);
        }
        else
        {
            LocationCache.Instance.SetCachedLocations(m_search.Locations);
        }

        Action onDone = () =>
            {
                CacheReady = true;

                if (Updated != null)
                {
                    Updated(this, EventArgs.Empty);
                }
            };

        DoStoryTagSearch(onDone, "ore");
        DoStoryTagSearch(onDone, "brick");
        DoStoryTagSearch(onDone, "wheat", "wood", "wool");
    }

    private void DoSearch(Coordinates coordinates)
    {
        IEnumerable<ILocationSearch> toCancel = null;

        lock (m_storyTagSearches)
        {
            toCancel = m_storyTagSearches;
            m_storyTagSearches = new List<ILocationSearch>();
        }

        if (toCancel != null)
        {
            foreach (var s in toCancel)
            {
                s.Abort();
            }
        }

        m_search.Search(coordinates, SearchRange, SearchCompleted);
    }

    void HandleSystemPositionUpdated (Coordinates coordinates)
    {
        if (ShouldSearch(coordinates)) {
            m_logger.Verbose("HandleSystemPositionUpdated - ShouldSearch: true");
            
            m_lastCoordinates = coordinates;

            if (m_search != null) {
                m_search.Abort();
                m_search = null;
            }
         
            m_search = m_searchProvider.CreateSearch();

            m_logger.Verbose("Searching");

            DoSearch(coordinates);
        }
    }
    
    bool ShouldSearch(Coordinates coords)
    {
        if (m_lastCoordinates != null) {
            return m_lastCoordinates.GetDistanceFrom(coords) > SearchMoveDistance;
        }
        
        return true;
    }
}
