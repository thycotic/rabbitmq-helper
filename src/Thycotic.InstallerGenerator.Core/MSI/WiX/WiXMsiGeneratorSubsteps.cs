namespace Thycotic.InstallerGenerator.Core.MSI.WiX
{
    /// <summary>
    /// Substeps used in WiX. There are more tools but this only uses a handful.
    /// </summary>
    /// <seealso cref="http://wixtoolset.org/documentation/manual/v3/overview/alltools.html"/>
    public class WiXMsiGeneratorSubsteps
    {
        /// <summary>
        /// Gets or sets the heat.
        /// </summary>
        /// <remarks>Generates WiX authoring from various input formats. It is used for harvesting files, 
        /// Visual Studio projects and Internet Information Server web sites, "harvesting" these files 
        /// into components and generating Windows Installer XML Source files (.wxs). 
        /// GetHeatPath is good to use when you begin authoring your first Windows Installer package for a product.</remarks>
        /// <value>
        /// The heat.
        /// </value>
        public string Heat { get; set; }

        /// <summary>
        /// Gets or sets the candle.
        /// </summary>
        /// <remarks>Preprocesses and compiles WiX source files into object files (.wixobj). 
        /// For more information on compiling, see Compiler. For more information on preprocessing, see Preprocessor.</remarks>
        /// <value>
        /// The candle.
        /// </value>
        public string Candle { get; set; }

        /// <summary>
        /// Gets or sets the light.
        /// </summary>
        /// <remarks>Links and binds one or more .wixobj files and creates a Windows Installer 
        /// database (.msi or .msm). When necessary, GetLightPath will also create cabinets and embed 
        /// streams into the Windows Installer database it creates. For more information on linking, see Linker.</remarks>
        /// <value>
        /// The light.
        /// </value>
        public string Light { get; set; }

    }
}