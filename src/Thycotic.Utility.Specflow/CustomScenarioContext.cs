using System;
using NSubstitute;
using TechTalk.SpecFlow;

namespace Thycotic.Utility.Specflow
{
    public class CustomScenarioContext
    {
        private readonly ScenarioContext _baseContext;

        public CustomScenarioContext(ScenarioContext baseContext)
        {
            _baseContext = baseContext;
        }

        public T Get<T>(string key)
        {
            return _baseContext.Get<T>(key);
        }

        public void SetNull(string key)
        {
            //yes, I know, it's backwards...
            _baseContext[key] = null;
        }

        public T Set<T>(string key, T data)
        {
            //yes, I know, it's backwards... -dkk
            _baseContext.Set(data, key);
            return data;
        }

        public object this[string key]
        {
            get { return _baseContext[key]; }
            set { _baseContext[key] = value; }
        }


        public T GetSubstituteFor<T>()
        where T : class
        {
            return Substitute.For<T>();
        }

        public T1 GetSubstituteFor<T1, T2>()
            where T1 : class
            where T2 : class
        {
            return Substitute.For<T1, T2>();
        }


        public T1 GetSubstituteFor<T1, T2, T3>()
            where T1 : class
            where T2 : class
            where T3 : class
        {
            return Substitute.For<T1, T2, T3>();
        }


        public T SetSubstitute<T>(string key)
            where T : class
        {
           return Set(key, GetSubstituteFor<T>());
        }

        public T SetSubstitute<T>(string key, Action<T> preparer)
            where T : class
        {
            var substitute = GetSubstituteFor<T>();
            preparer.Invoke(substitute);
            return Set(key, substitute);
        }

        public T1 SetSubstitute<T1, T2>(string key)
            where T1 : class
            where T2 : class
        {
            return Set(key, GetSubstituteFor<T1, T2>());
        }

        public T1 SetSubstitute<T1, T2>(string key, Action<T1> preparer)
            where T1 : class
            where T2 : class
        {
            var substitute = GetSubstituteFor<T1, T2>();
            preparer.Invoke(substitute);
            return Set(key, substitute);
        }

        public T1 SetSubstitute<T1, T2, T3>(string key)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            return Set(key, GetSubstituteFor<T1, T2, T3>());
        }

        public T1 SetSubstitute<T1, T2, T3>(string key, Action<T1> preparer)
            where T1 : class
            where T2 : class
            where T3 : class
        {
            var substitute = GetSubstituteFor<T1, T2, T3>();
            preparer.Invoke(substitute);
            return Set(key, substitute);
        }
    }
}
