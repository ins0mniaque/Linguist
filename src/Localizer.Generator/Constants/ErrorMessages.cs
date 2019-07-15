namespace Localizer.Generator
{
    internal static class ErrorMessages
    {
        public const string UnknownResourceFileFormat    = "Could not determine resource file format for '{0}'.";
        public const string PropertyAlreadyExists        = "The resource support class already contains a property named '{0}'.";
        public const string InvalidPropertyType          = "Invalid type '{0}' for resource named '{1}'.";
        public const string CannotCreateResourceProperty = "Cannot create property for resource named '{0}'.";
        public const string SkippingWinFormsResource     = "Skipping Windows Forms localization resource named '{0}'.";
        public const string ErrorInStringResourceFormat  = "Error in format for resource named '{1}': {0}";
        public const string CannotCreateFormatMethod     = "Cannot create the format method '{0}' for resource named '{1}'.";
    }
}