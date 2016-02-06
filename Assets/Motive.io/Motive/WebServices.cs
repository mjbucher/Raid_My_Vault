using UnityEngine;
using System.Collections;
using Motive.Core.WebServices;
using System;
using Motive.Core.Models;
using System.IO;
using Motive.Core.Diagnostics;
using Motive.Unity.Utilities;
using Motive.Core.Scripting;
using Motive.AR.LocationServices;
using Motive.Core.Media;
using Motive.AR.Scripting;
using Motive.AR.WeatherServices;
using Motive.Core.Storage;
using Motive.Core.Social;

public class WebServices : SingletonComponent<WebServices>
{

    public string MotiveUrl = "https://alpha.motive.io";

    public string AppName = "YOUR APP NAME";
    public string ApiKey = "YOUR API KEY";
    public string SpaceName = "YOUR SPACE NAME";
    public string UserDomain;
    public string ActivityFeed;

    public string CharacterCatalog = "catan_characters";
    public string ResourceCollectibleCatalog = "catan_resources";
    public string BonusCardCollectibleCatalog = "catan_bonus_cards";
    public string DevelopmentCardCollectibleCatalog = "catan_development_cards";
    public string ScriptCatalog = "catan_scripts";
    public string LocationStoryTagCatalog = "catan_story_tags";
    public string LocationTreasureChestCatalog = "catan_treasure_chests";
    public string AnnotationMarkerCatalog = "catan_annotation_markers";

    public string FoursquareClientId;
    public string FoursquareClientSecret;
    public string ForecastIOApiKey;

    public bool UseUnpublishedLatest = false;
    public MediaDownloadManager MediaDownloadManager { get; private set; }
    public ILocationSearchProvider LocationSearchProvider { get; private set; }
    public IWeatherService WeatherService { get; private set; }
    public UserManager UserManager { get; private set; }
    public string FullUserDomain { get
        {
            return SpaceName + "." + UserDomain;
        }
    }

    private CatalogLoader m_catalogLoader;
	private Motive.Core.Diagnostics.Logger m_logger;

    private bool m_initialized;
    private int m_waitingCatalogCount;
    private bool m_waitingForCatalogs;
    private bool m_catalogsMediaReady;
    private int m_totalFiles;
    private long m_totalSize;

    private Catalog<Script> m_scriptCatalog;
    private Catalog<FoursquareCategoryMap> m_foursquareCategoryMap;
    private Catalog<StoryTagLocationType> m_storyTagLocationType;

    private LoadingPanel m_loadingPanel;
    public void LoadCatalog<T>(string spaceName, string catalogName, bool useUnpublishedLatest, Action<Catalog<T>> onLoad)
    {
        if (string.IsNullOrEmpty(catalogName))
        {
            return;
        }

        m_waitingCatalogCount++;

        var fileName = StorageManager.GetCatalogFileName(catalogName + ".json");

        m_catalogLoader.LoadCatalog<T>(fileName, spaceName, catalogName, useUnpublishedLatest,
            (success, catalog) =>
            {
                if (success)
                {
                    m_logger.Debug("Loaded catalog {0} with {1} item(s)",
                        catalogName, catalog.Items == null ? 0 : catalog.Items.Length);

                    // This callback happens outside of the Unity thread,
                    // use the Thread Helper to move into the Unity context
                    ThreadHelper.Instance.CallOnMainThread(() =>
                    {
                        MediaDownloadManager.AddMediaItemProvider(catalog);

                        onLoad(catalog);

                        // Since we're in the Unity thread here we don't need
                        // to protect this in a critical section
                        m_waitingCatalogCount--;
                    });
                }
                else
                {
                    m_logger.Error("Error loading catalog {0}", catalogName);

                    if (m_loadingPanel)
                    {
                        ThreadHelper.Instance.CallOnMainThread(() =>
                        {
                            m_loadingPanel.StatusText.text = "Error loading catalog " + catalogName;
                        });
                    }
                }
            });
    }

    public void LoadCatalog<T>(string catalogName, Action<Catalog<T>> onLoad)
    {
        LoadCatalog<T>(SpaceName, catalogName, UseUnpublishedLatest, onLoad);
    }

    void InitializeFoursquare(
        Catalog<FoursquareCategoryMap> categoryMap,
        Catalog<StoryTagLocationType> storyTagMap)
    {
        m_logger.Debug("Initializing Foursquare");

        // The Foursquare service uses is own folder to cache data
        var fsPath = StorageManager.EnsureFolder("foursquare");

        // Use a different catalog here that's not affected by the
        // "useUnpublishedLatest" flag.

        FoursquareService.Instance.Initialize(
            FoursquareClientId,
            FoursquareClientSecret,
            categoryMap,
            storyTagMap,
            (success) =>
            {
                m_logger.Debug("Callback from Foursquare: {0}", success);

                if (success)
                {
                    // The Foursquare service is initialized, we can
                    // start the cache driver. This keeps a local cache
                    // of locations available to the app.
                    LocationCacheDriver.Instance.Initialize();

                    // Set up a location search provider
                    LocationSearchProvider = FoursquareService.Instance.GetSearchProvider(FoursquareSearchIntent.Browse);

                    // Set this up as the default provider. This is used by
                    // location conditions in the AR stack if WebServiceLocationSource
                    // is chosen.
                    LocationSearchProviderRegistry.Instance.Register("default", LocationSearchProvider);
                }
            });
    }

    // Use this for initialization
    public void Initialize()
    {
#if !UNITY_WP8
        System.Net.ServicePointManager.ServerCertificateValidationCallback =
            new System.Net.Security.RemoteCertificateValidationCallback(
                (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
             System.Security.Cryptography.X509Certificates.X509Chain chain,
             System.Net.Security.SslPolicyErrors sslPolicyErrors) =>
                {
                    return true;
                });
#endif
        // Initialize Forecast.io weather services
        if (!string.IsNullOrEmpty(ForecastIOApiKey))
        {
            WeatherService = new ForecastIOWeatherService(ForecastIOApiKey);

            WeatherMonitor.Instance.Initialize(WeatherService);
        }

        MotiveAuthenticator.Instance.Initialize(
            MotiveUrl, 
            AppName, 
            ApiKey, 
            new FileStorageAgent(StorageManager.GetFilePath("authenticator", "authenticationState.json")));

        if (!string.IsNullOrEmpty(ActivityFeed))
        {
            UserActivityService.Instance.Initialize(MotiveUrl, SpaceName, ActivityFeed);
        }

        UserManager = new UserManager(MotiveUrl);

        ReloadFromServer();

        m_initialized = true;
    }

    void ReloadFromServer()
    {
        if (m_waitingForCatalogs)
        {
            // If we're already waiting, return
            return;
        }

        m_catalogsMediaReady = false;

        MediaDownloadManager = new MediaDownloadManager(StorageManager.EnsureFolder("media"));

        m_loadingPanel = PanelManager.Instance.Show<LoadingPanel>();

        m_waitingForCatalogs = true;

		m_logger = new Motive.Core.Diagnostics.Logger(this);

        m_catalogLoader = new CatalogLoader(MotiveUrl, SpaceName, UseUnpublishedLatest);

        LoadCatalog<Script>(ScriptCatalog, (catalog) =>
        {
            m_scriptCatalog = catalog;
        });

        LoadCatalog<Collectible>(ResourceCollectibleCatalog, (catalog) =>
        {
            CollectibleDirectory.Instance.PopulateResources(catalog);
        });

        LoadCatalog<Collectible>(BonusCardCollectibleCatalog, (catalog) =>
        {
            CollectibleDirectory.Instance.PopulateBonusCards(catalog);
        });

        LoadCatalog<Character>(CharacterCatalog, (catalog) =>
        {
            CharacterDirectory.Instance.Populate(catalog);
        });

        LoadCatalog<LocationTreasureChest>(LocationTreasureChestCatalog, (catalog) =>
        {
            LocationTreasureChestManager.Instance.Populate(catalog);
        });

        LoadCatalog<StoryTagLocationType>(LocationStoryTagCatalog, (catalog) =>
        {
            m_storyTagLocationType = catalog;
        });

        LoadCatalog<AnnotationMarker>(AnnotationMarkerCatalog, (catalog) =>
        {
            MapController.Instance.SetAnnotationMarkerCatalog(catalog);
        });

        LoadCatalog<FoursquareCategoryMap>("motive.ar", "foursquare_category_map", false, (catalog) =>
        {
            m_foursquareCategoryMap = catalog;
        });
    }

    public void Nuke()
    {
        ScriptManager.Instance.Abort();

        StorageManager.DeleteGameFolder();

        Application.Quit();
    }

    public void Reset()
    {
        Reset(false);
    }

    public void Reset(bool skipReload)
    {
        ScriptManager.Instance.Reset(() =>
        {
            if (skipReload)
            {
                ScriptManager.Instance.RunScripts();
            }
            else
            {
                ReloadFromServer();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_initialized)
        {
            return;
        }

        if (m_waitingForCatalogs && m_waitingCatalogCount == 0)
        {
            m_waitingForCatalogs = false;

            Action ready = () =>
            {
                InitializeFoursquare(m_foursquareCategoryMap, m_storyTagLocationType);

                if (m_loadingPanel)
                {
                    m_loadingPanel.Back();
                }

                m_catalogsMediaReady = true;
            };

            // Catalogs ready! Download media if required
            MediaDownloadManager.Resolve((resSuccess) =>
            {
                ThreadHelper.Instance.CallOnMainThread(() =>
                {
                    if (resSuccess)
                    {
                        if (MediaDownloadManager.OutstandingFileSize > 0)
                        {
                            m_totalFiles = MediaDownloadManager.OutstandingFileCount;
                            m_totalSize = MediaDownloadManager.OutstandingFileSize;

                            if (m_loadingPanel)
                            {
                                m_loadingPanel.DownloadSizeText.gameObject.SetActive(true);
                                m_loadingPanel.DownloadFilesText.gameObject.SetActive(true);
                            }

                            MediaDownloadManager.DownloadAll((dlSuccess) =>
                            {
                                ThreadHelper.Instance.CallOnMainThread(() =>
                                {
                                    if (!dlSuccess)
                                    {
                                        m_loadingPanel.StatusText.text = "Error";
                                    }
                                    else
                                    {
                                        ready();
                                    }
                                });
                            });
                        }
                        else
                        {
                            ready();
                        }
                    }
                    else
                    {
                        m_logger.Error("Failed to resolve media.");
                    }
                });
            });
        }

        if (m_loadingPanel != null && MediaDownloadManager.OutstandingFileSize > 0)
        {
            m_loadingPanel.StatusText.text = "Downloading Media";
            m_loadingPanel.DownloadFilesText.text = string.Format("{0}/{1} files",
                MediaDownloadManager.OutstandingFileCount, m_totalFiles);

            m_loadingPanel.DownloadSizeText.text = string.Format("{0}/{1}k",
                ((double)MediaDownloadManager.OutstandingFileSize / 1000),
                ((double)m_totalSize / 1000));
        }

        if (m_catalogsMediaReady &&
            m_scriptCatalog != null &&
            LocationCacheDriver.Instance.CacheReady)
        {
            ScriptManager.Instance.RunScriptCatalog(m_scriptCatalog);
            m_scriptCatalog = null;
        }
    }
}
