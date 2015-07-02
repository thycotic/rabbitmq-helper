using System;

namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    public abstract class TestBase<TSut> : ISingleClassContractTests, IGivenWhenThenTests
    {
        public TSut Sut { get; set; }

        public virtual void Setup()
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

        public void ShouldFail<TException>(string message, Func<object> func)
        {
            try
            {
                func.Invoke();

                throw new Exception("Exception was expected");
            }
            catch (Exception ex)
            {
                if (ex is TException)
                {
                    if (ex.Message != message)
                    {
                        throw new Exception(string.Format("Exception was cause but had the wrong message. Expected \"{0}\". Got \"{1}\"", message, ex.Message), ex);
                    }

                }
                else
                {
                    throw new Exception(string.Format("Exception is not of the correct type. Expected {0}. Got {1}", typeof(TException).FullName, ex.GetType().FullName), ex);
                }

            }
        }
    }
}
