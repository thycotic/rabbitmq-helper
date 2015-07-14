namespace Thycotic.Utility.Testing.CodeContracts
{
    public interface ISingleClassContractTests
    {
        /// <summary>
        /// Ensures that the constructor for the class under test verifies parameters for validity
        /// </summary>
        void ConstructorParametersDoNotExceptInvalidParameters();

    }
}
