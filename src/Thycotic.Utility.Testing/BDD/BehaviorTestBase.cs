using System;
using Thycotic.Utility.Tests.CodeContracts;

namespace Thycotic.Utility.Tests.BDD
{
    /// <summary>
    /// Base Behavior-Driven test class. http://martinfowler.com/bliki/GivenWhenThen.html/// </summary>
    /// <typeparam name="TSut">The type of the sut.</typeparam>
    public abstract class BehaviorTestBase<TSut> : ISingleClassContractTests, ICustomTestFixture
    {
        public TSut Sut { get; set; }
        
        public virtual void SetUp()
        {
        }

        public virtual void TearDown()
        {
        }

        public abstract void ConstructorParametersDoNotExceptInvalidParameters();

        public void Given(Action promises)
        {
            try
            {
                promises.Invoke();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Given clause failed", ex);
            }

        }

        public void When(Action actions)
        {
            try
            {
                actions.Invoke();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("When clause failed", ex);
            }
        }

        public void Then(Action assertions)
        {
            try
            {
                assertions.Invoke();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Then clause failed", ex);
            }
        }

        
    }
}
