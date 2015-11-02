namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// Represents the content in an assembly info file.
    /// </summary>
    public sealed class AssemblyInfoParseResult
    {
        private readonly bool _clsCompliant;
        private readonly string _company;
        private readonly bool _comVisible;
        private readonly string _configuration;
        private readonly string _copyright;
        private readonly string _description;
        private readonly string _assemblyFileVersion;
        private readonly string _guid;
        private readonly string _assemblyInformationalVersion;
        private readonly string _internalsVisibleTo;
        private readonly string _product;
        private readonly string _title;
        private readonly string _trademark;
        private readonly string _assemblyVersion;

        /// <summary>
        /// Gets a value indicating whether the assembly is CLSCompliant.
        /// </summary>
        /// <value>The assembly CLSCompliant attribute.</value>
        public bool ClsCompliant
        {
            get { return _clsCompliant; }
        }

        /// <summary>
        /// Gets the assembly Company Attribute.
        /// </summary>
        /// <value>The assembly Company attribute.</value>
        public string Company
        {
            get { return _company; }
        }

        /// <summary>
        /// Gets a value indicating whether the assembly is ComVisible
        /// </summary>
        /// <value>The assembly ComVisible attribute.</value>
        public bool ComVisible
        {
            get { return _comVisible; }
        }

        /// <summary>
        /// Gets the assembly Configuration Attribute.
        /// </summary>
        /// <value>The assembly Configuration attribute.</value>
        public string Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Gets the assembly Copyright Attribute.
        /// </summary>
        /// <value>The assembly Copyright attribute.</value>
        public string Copyright
        {
            get { return _copyright; }
        }

        /// <summary>
        /// Gets the assembly Description Attribute.
        /// </summary>
        /// <value>The assembly Description attribute.</value>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets the assembly file version.
        /// </summary>
        /// <value>The assembly file version.</value>
        public string AssemblyFileVersion
        {
            get { return _assemblyFileVersion; }
        }

        /// <summary>
        /// Gets the assembly Guid Attribute.
        /// </summary>
        /// <value>The assembly Guid attribute.</value>
        public string Guid
        {
            get { return _guid; }
        }

        /// <summary>
        /// Gets the assembly informational version.
        /// </summary>
        /// <value>The assembly informational version.</value>
        public string AssemblyInformationalVersion
        {
            get { return _assemblyInformationalVersion; }
        }

        /// <summary>
        /// Gets the assembly InternalsVisibleTo Attribute.
        /// </summary>
        /// <value>The assembly InternalsVisibleTo attribute.</value>
        public string InternalsVisibleTo
        {
            get { return _internalsVisibleTo; }
        }

        /// <summary>
        /// Gets the assembly Product Attribute.
        /// </summary>
        /// <value>The assembly Product attribute.</value>
        public string Product
        {
            get { return _product; }
        }

        /// <summary>
        /// Gets the assembly Title Attribute.
        /// </summary>
        /// <value>The assembly Title attribute.</value>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets the assembly Trademark Attribute.
        /// </summary>
        /// <value>The assembly Trademark attribute.</value>
        public string Trademark
        {
            get { return _trademark; }
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public string AssemblyVersion
        {
            get { return _assemblyVersion; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfoParseResult"/> class.
        /// </summary>
        /// <param name="clsCompliant">The assembly CLSCompliant Attribute.</param>
        /// <param name="company">The assembly Company Attribute.</param>
        /// <param name="comVisible">The assembly ComVisible Attribute.</param>
        /// <param name="configuration">The assembly Configuration Attribute.</param>
        /// <param name="copyright">The assembly Copyright Attribute.</param>
        /// <param name="description">The assembly Description Attribute.</param>
        /// <param name="assemblyFileVersion">The assembly file version.</param>
        /// <param name="guid">The assembly Guid Attribute.</param>
        /// <param name="assemblyInformationalVersion">The assembly informational version.</param>
        /// <param name="internalsVisibleTo">The assembly InternalsVisibleTo Attribute.</param>
        /// <param name="product">The assembly Product Attribute.</param>
        /// <param name="title">The assembly Title Attribute.</param>
        /// <param name="trademark">The assembly Trademark Attribute.</param>
        /// <param name="assemblyVersion">The assembly version.</param>
        public AssemblyInfoParseResult(string clsCompliant,
            string company,
            string comVisible,
            string configuration,
            string copyright,
            string description,
            string assemblyFileVersion,
            string guid,
            string assemblyInformationalVersion,
            string internalsVisibleTo,
            string product,
            string title,
            string trademark,
            string assemblyVersion)
        {
            _clsCompliant = !string.IsNullOrWhiteSpace(clsCompliant) && bool.Parse(clsCompliant);
            _company = company;
            _comVisible = !string.IsNullOrWhiteSpace(clsCompliant) && bool.Parse(comVisible);
            _configuration = configuration;
            _copyright = copyright;
            _description = description;
            _assemblyFileVersion = assemblyFileVersion;
            _guid = guid;
            _assemblyInformationalVersion = assemblyInformationalVersion;
            _internalsVisibleTo = internalsVisibleTo;
            _product = product;
            _title = title;
            _trademark = trademark;
            _assemblyVersion = assemblyVersion;
        }
    }
}