﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HuntingModel.Localization {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class HomeRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal HomeRes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HuntingModel.Localization.HomeRes", typeof(HomeRes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Zde budou podstatné informace o fungování aplikace, možná i nějaké obrázky.
        /// </summary>
        public static string LABEL_ABOUT {
            get {
                return ResourceManager.GetString("LABEL_ABOUT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Navštěvované revíry:.
        /// </summary>
        public static string LABEL_HUNTER {
            get {
                return ResourceManager.GetString("LABEL_HUNTER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Spravované revíry:.
        /// </summary>
        public static string LABEL_STEWARD {
            get {
                return ResourceManager.GetString("LABEL_STEWARD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aplikace {0} slouží ke správě revírů..
        /// </summary>
        public static string LABEL_TERRITORY_INFO {
            get {
                return ResourceManager.GetString("LABEL_TERRITORY_INFO", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Začněte přidáním revíru nebo kontaktujte jeho správce..
        /// </summary>
        public static string LABEL_TERRITORY_LOGGED {
            get {
                return ResourceManager.GetString("LABEL_TERRITORY_LOGGED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pro kontaktování správce revíru nebo přidání vlastního se přihlašte..
        /// </summary>
        public static string LABEL_TERRITORY_LOGIN {
            get {
                return ResourceManager.GetString("LABEL_TERRITORY_LOGIN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Revíry.
        /// </summary>
        public static string TITLE_TERRITORY {
            get {
                return ResourceManager.GetString("TITLE_TERRITORY", resourceCulture);
            }
        }
    }
}
