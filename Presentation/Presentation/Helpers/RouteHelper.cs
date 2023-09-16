using System;
namespace Presentation.Helpers;

public class RouteHelper
{
    //PRE 
    public const string APP_HOME = "AppRegistration";

    //MAIN
    public const string APPLICANT = "Applicant";

    //Remote Script Path
    public const string COMPO_UI_PATH = "~/_content/SDMS.UI.Components/";
    public const string ADMIN_UI_MODULES_PATH = "~/_content/SDMS.UI.Admin.Modules/";
    public const string DATA_UI_MODULES_PATH = "~/_content/SDMS.UI.Data.Modules/";
    public const string SHOP_UI_MODULES_PATH = "~/_contentSDMS.UI.Shop.Modules/";

    //Account
    public const string ACCOUNT = "account";
    public const string ACCOUNT_LOGIN = "sign-in";
    public const string ACCOUNT_LOGOUT = "sign-out";


 

    //Areas
    public const string PORTAL_AREA = "portal";
    public const string STORE_SETUP_AREA = "storesetup";
    public const string CATALOGUE_WORKFLOW_AREA = "catworkflow";




    //Common
    public const string DASHBOARD = "dashboard";


    //Dashboard Routes 
    public const string PORTAL_DASHBOARD = "portal-services";
    public const string DICTIONARY_DASHBOARD = "dictionary-services";
    public const string CATALOGUE_DASHBOARD = "data-catalogs";

   
   
    //Area Routes

    //Catalogue Routes
    public const string CATALOGUE_SECTORS = "sectors";
    public const string CATALOGUE_CATEGORIES = "categories";
    public const string CATALOGUE_TABLES = "tables";
    public const string CATALOGUE_INDICATORS = "indicators";

    //Catalogue WorkflowRoutes
    public const string CAT_WORKFLOW_MODIFY_REQUESTS = "modify-requests";
    public const string CAT_WORKFLOW_MODIFY_AUTHS = "categories";
    public const string CAT_WORKFLOW_ARCHIVE_REQUESTS = "archive-requests";
    public const string CAT_WORKFLOW_ARCHIVE_AUTHS = "indicators";

    //Workflow Routes
    public const string CATALOGUE_ENTITY_REQUESTS = "entity-modification-requests";
    public const string CATALOGUE_ENTITY_APPROVALS = "entity-modification-approvals";

    public const string CATALOGUE_MODIFICATION = "entity-modification";
    public const string CATALOGUE_ARCHIVE = "entity-archives";
    public const string CATALOGUE_RESTORE = "entity-restore";
    public const string CATALOGUE_LOCK = "entity-lock";
    public const string CATALOGUE_UNLOCK = "entity-unlock";

    //Report Reoutes
    public const string CATALOGUE_AUDIT_REPORTS = "audit-reports";


    //Portal Routes
    public const string PORTAL_ROLES = "roles";
    public const string PORTAL_USERS = "users";
    public const string PORTAL_PERMISSIONS = "permissions";
    public const string PORTAL_MODULES = "modules";
    public const string PORTAL_MODULE_CATEGORIES = "module-categories";
    public const string PORTAL_AUDITS = "audits";

    //Store-Setup Routes
    public const string SETUP_BOOK_CATEGORIES = "book-categories";
    public const string DICTIONARY_DATA_ENTITIES = "data-entities";
    public const string DICTIONARY_DATA_SOURCES = "data-sources";
    public const string DICTIONARY_UNITS = "units";






    //Collaboration Routes
    public const string COLLAB_TEAMS = "teams";
    public const string COLLAB_COLLABS = "collabs";


    //Authorization 

    public const string DATA_AUTHORIZE = "data-authorize";

    //CMS
    public const string QUICK_FLASHES_CMS = "quick-flashes";
    public const string FLASH_CARDS_CMS = "flash-cards";
    public const string FLASH_CARD_ENTITIES_CMS = "flash-card-entities";
    public const string FLASH_CARD_DATA_CMS = "flash-card-data";
    public const string ANALYTIC_CONTENTS_CMS = "analytic-contents";
    public const string ANALYTIC_DATA_CONTENTS_CMS = "analytic-data";
    public const string FEATURED_CONTENT_CMS = "featured-contents";
    public const string RELEASE_CALENDARS_CMS = "release-calendars";
    public const string CUSTOMER_FEEDBACK_CMS = "feed-back";



    public const string COUNTRY_PROFILE_CMS = "country-profile";
    public const string FLASH_CARD_MANAGER = "flash-card-manager";
    public const string FLASH_DATA_CARD_CMS = "";
    public const string FLASH_CARD_HELPER = "";

    //OData
    public const string ODATA_DATAVIEW_HELPER = "dataviews-helper";




    public const string PICKUP_SETUP_CATEGORIES = "pick-up";
    public const string Home = "Home";
}
