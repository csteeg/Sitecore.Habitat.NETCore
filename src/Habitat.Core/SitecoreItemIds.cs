using System;

namespace Habitat.Core
{
    /// <summary>
    /// Implements a static class for holding assorted
    ///             well-known GUID values.
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// The class does not contain any functionality, but serves as
    ///             a placeholder for a predefined GUIDs.
    /// 
    /// </remarks>
    public static class SitecoreItemIds
    {
        /// <summary>
        /// The Anonymous user ID.
        /// </summary>
        public static readonly Guid AnonymousUser = new Guid("{4AF789F7-750F-45C1-B4F0-669A6348482E}");
        /// <summary>
        /// The ID of the /sitecore/system/Settings/Rules/Conditional Renderings/Global Rules.
        /// </summary>
        public static readonly Guid ConditionalRenderingsGlobalRules = new Guid("{6892B190-D0C8-4628-A179-24D197AB0C07}");
        /// <summary>
        /// The ID of "/sitecore/content"
        /// </summary>
        public static readonly Guid ContentRoot = new Guid("{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}");
        /// <summary>
        /// The ID of "/sitecore/content/Applications/Content Editor/Ribbons/Ribbons/Default"
        /// </summary>
        public static readonly Guid DefaultRibbon = new Guid("{073BBB5D-65B5-485F-A1F8-64E55C84696E}");
        /// <summary>
        /// The ID of "/sitecore/layout/Devices"
        /// </summary>
        public static readonly Guid DevicesRoot = new Guid("{E18F4BC6-46A2-4842-898B-B6613733F06F}");
        /// <summary>
        /// The ID of "/sitecore/system/Dictionary"
        /// </summary>
        public static readonly Guid Dictionary = new Guid("{504AE189-9F36-4C62-9767-66D73D6C3084}");
        /// <summary>
        /// The ID of the 'Everyone' role.
        /// </summary>
        public static readonly Guid EveryoneRoleID = new Guid("{00088163-665D-4F6F-9E63-C0CF1FB4E2FE}");
        /// <summary>
        /// The ID of the '/sitecore/system/language' item.
        /// </summary>
        public static readonly Guid LanguageRoot = new Guid("{64C4F646-A3FA-4205-B98E-4DE2C609B60F}");
        /// <summary>
        /// The ID of "/sitecore/layout"
        /// </summary>
        public static readonly Guid LayoutRoot = new Guid("{EB2E4FFD-2761-4653-B052-26A64D385227}");
        /// <summary>
        /// The ID of "/sitecore/layout/layouts"
        /// </summary>
        public static readonly Guid Layouts = new Guid("{75CC5CE4-8979-4008-9D3C-806477D57619}");
        /// <summary>
        /// The ID of "/sitecore/templates/branches"
        /// </summary>
        public static readonly Guid BranchesRoot = new Guid("{BAD98E0E-C1B5-4598-AC13-21B06218B30C}");
        /// <summary>
        /// The ID of "/sitecore/templates/branches"
        /// </summary>
        [Obsolete("Deprecated - Use BranchesRoot instead.")]
        public static readonly Guid MastersRoot = new Guid("{BAD98E0E-C1B5-4598-AC13-21B06218B30C}");
        /// <summary>
        /// The ID of "/sitecore/media library"
        /// </summary>
        public static readonly Guid MediaLibraryRoot = new Guid("{3D6658D8-A0BF-4E75-B3E2-D050FABCF4E1}");
        /// <summary>
        /// The Null ID.
        /// </summary>
        public static readonly Guid Null = new Guid("{00000000-0000-0000-0000-000000000000}");
        /// <summary>
        /// The ID of "/sitecore/layout/Placeholders"
        /// </summary>
        public static readonly Guid PlaceholderSettingsRoot = new Guid("{1CE3B36C-9B0C-4EB5-A996-BFCB4EAA5287}");
        /// <summary>
        /// The ID of the Sitecore root item.
        /// </summary>
        public static readonly Guid RootID = new Guid("{11111111-1111-1111-1111-111111111111}");
        /// <summary>
        /// The ID of the root item of the system/policies section
        /// </summary>
        public static readonly Guid Policies = new Guid("{1E7C8D5A-51CF-42A7-8D58-0752B3E39C8B}");
        /// <summary>
        /// The ID of the root item of the templates section
        /// </summary>
        public static readonly Guid TemplateRoot = new Guid("{3C1715FE-6A13-4FCF-845F-DE308BA9741D}");
        /// <summary>
        /// The ID of the root item of the system/workflows section
        /// </summary>
        public static readonly Guid WorkflowRoot = new Guid("{05592656-56D7-4D85-AACF-30919EE494F9}");
        /// <summary>
        /// The ID of the /sitecore/content/System/Shell item in the core database
        /// </summary>
        public static readonly Guid Shell = new Guid("{4616E2BE-BF68-4D22-91B3-93301C9F86B7}");
        /// <summary>
        /// The ID of the /sitecore/content/System/Shell/__All in the core database
        /// </summary>
        public static readonly Guid ShellAll = new Guid("{DF4F23E3-9BAC-42D6-A249-E50CA7475FFD}");
        /// <summary>
        /// The ID of the /sitecore/content/System/Shell/__Default in the core database
        /// </summary>
        public static readonly Guid ShellDefault = new Guid("{A8653DDD-862E-418F-A312-BD543157E354}");
        /// <summary>
        /// The ID of "/sitecore/system"
        /// </summary>
        public static readonly Guid SystemRoot = new Guid("{13D6D6C6-C50B-4BBD-B331-2B04F1A58F21}");
        /// <summary>
        /// The ID of the "/sitecore/system/Virtual Structures" item
        /// </summary>
        [Obsolete("This ID has been deprecated.")]
        public static readonly Guid VirtualStructures = new Guid("{6542A6DC-2859-4041-8C3E-E356BF390DC9}");
        /// <summary>
        /// ID indicating 'undefined' value.
        /// </summary>
        public static readonly Guid Undefined = new Guid("{FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF}");

        /// <summary>
        /// Defines the analytics class.
        /// 
        /// </summary>
        public static class Analytics
        {
            /// <summary>
            /// ID indicating default condition
            /// </summary>
            public static readonly Guid DefaultCondition = new Guid("{00000000-0000-0000-0000-000000000000}");
            /// <summary>
            /// ID indicating '/sitecore/system/Settings/Analytics/Filters/Criteria' value.
            /// </summary>
            public static readonly Guid Criteria = new Guid("{8463C2CD-2829-4B44-80C8-0B09B391D696}");
            /// <summary>
            /// ID indicating '/sitecore/system/Marketing Control Panel' value.
            /// </summary>
            public static readonly Guid MarketingCenterItem = new Guid("{33CFB9CA-F565-4D5B-B88A-7CDFE29A6D71}");
            /// <summary>
            /// ID indicating '/sitecore/system/settings/Analytics/Filters/Macros' value.
            /// </summary>
            public static readonly Guid Macros = new Guid("{1B4FAD92-BCB4-4842-A8F0-23215634A0E4}");
            /// <summary>
            /// ID indicating '/sitecore/system/settings/Analytics/Page Events' value.
            /// </summary>
            public static readonly Guid PageEvents = new Guid("{633273C1-02A5-4EBC-9B82-BD1A7C684FEA}");
            /// <summary>
            /// ID indicating '/sitecore/system/settings/Analytics/Profiles' value.
            /// </summary>
            public static readonly Guid Profiles = new Guid("{12BD7E35-437B-449C-B931-23CFA12C03D8}");
            /// <summary>
            /// ID indicating '/sitecore/system/Settings/Analytics/Reports/Reports' value.
            /// </summary>
            public static readonly Guid GlobalReports = new Guid("{63AE78FF-7DAA-4B80-9A93-0D8145269716}");
            /// <summary>
            /// ID indicating '/sitecore/system/settings/Analytics/Visitor Identifications' value.
            /// </summary>
            public static readonly Guid VisitorIdentifications = new Guid("{C0BE89B5-4061-4276-9C4F-569EDC4EEF06}");
            /// <summary>
            /// ID indicating '/sitecore/system/settings/Analytics/Visitor Identification Types' value.
            /// </summary>
            public static readonly Guid VisitorIdentificationTypes = new Guid("{220E8575-DA98-4F87-97A6-34940BEA0109}");
            /// <summary>
            /// ID indicating '/sitecore/system/Settings/Analytics/Dashboard Reports' value.
            /// </summary>
            public static readonly Guid DashboardReportsItem = new Guid("{3A81F528-EAC1-4B33-90C0-B261584855DD}");
            /// <summary>
            /// ID indicating '/sitecore/system/Settings/Analytics/Traffic Type' value.
            /// </summary>
            public static readonly Guid TrafficTypesItem = new Guid("{7E265978-8C1B-419D-BC06-0B5D101F04DF}");
            /// <summary>
            /// ID indicating '/sitecore/system/Settings/Analytics/Organic Branded Keywords'
            /// </summary>
            public static readonly Guid OrganicBrandedKeywords = new Guid("{B9985DD1-D81A-43BB-B7F7-C1294FEC09F4}");

            /// <summary>
            /// Defines the reports class.
            /// 
            /// </summary>
            public static class Reports
            {
                /// <summary>
                /// ID indicating '/sitecore/system/settings/Analytics/Reports/Item reports' value.
                /// </summary>
                public static readonly Guid ItemReports = new Guid("{B62E6C0C-8BE3-4F51-939A-DB039EEFA3A4}");
            }

            /// <summary>
            /// Defines the reports class.
            /// 
            /// </summary>
            public static class MarketingCenter
            {
                /// <summary>
                /// ID indicating '/sitecore/system/Marketing Control Panel/Campaigns' value.
                /// </summary>
                public static readonly Guid Campaigns = new Guid("{EC095310-746F-4C1B-A73F-941863564DC2}");
                /// <summary>
                /// ID indicating '/sitecore/system/Marketing Control Panel/Goals' value.
                /// </summary>
                public static readonly Guid Goals = new Guid("{0CB97A9F-CAFB-42A0-8BE1-89AB9AE32BD9}");
                /// <summary>
                /// ID indicating '/sitecore/system/Marketing Control Panel/Presentation/Policies' value.
                /// </summary>
                public static readonly Guid Policies = new Guid("{DB40C9D3-5DB3-4831-A0A9-53AB17EED652}");
                /// <summary>
                /// ID indicating "/sitecore/system/Marketing Control Panel/Reports/Report Filters" value.
                /// </summary>
                public static readonly Guid ReportFilters = new Guid("{5B3FE22D-1DC9-4CC4-9665-1A3F2DDD6732}");
                /// <summary>
                /// ID indicating '/sitecore/system/Marketing Control Panel/Test Laboratory' value.
                /// </summary>
                public static readonly Guid TestLaboratory = new Guid("{BA1B87AC-0853-45F0-AE13-41F969540134}");
            }
        }
    }
}
