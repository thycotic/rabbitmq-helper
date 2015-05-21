﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.34209
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Thycotic.MessageQueue.Client.Wrappers.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("BasicConsumerWrapper")]
    public partial class BasicConsumerWrapperFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "BasicConsumerWrapper.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "BasicConsumerWrapper", "", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 4
#line 5
 testRunner.Given("there exists a substitute object for ICommonConnection stored in the scenario as " +
                    "CommonConnectionTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.And("there exists a substitute object for IExchangeNameProvider stored in the scenario" +
                    " as ExchangeNameProviderTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 7
 testRunner.And("there exists a substitute object for IBasicConsumer<BasicConsumableDummy> stored " +
                    "in the scenario as BasicConsumerTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 8
 testRunner.And("there exists a substitute object for Owned<IBasicConsumer<BasicConsumableDummy>> " +
                    "stored in the scenario as OwnedBasicConsumerTest which returns IBasicConsumer<Ba" +
                    "sicConsumableDummy> BasicConsumerTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 9
 testRunner.And("there exists a basic consumer factory function stored in the scenario as Consumer" +
                    "FactoryTest which returns Owned<IBasicConsumer<BasicConsumableDummy>> OwnedBasic" +
                    "ConsumerTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 10
 testRunner.And("there exists a substitute object for IObjectSerializer stored in the scenario as " +
                    "ObjectSerializerTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
 testRunner.And("there exists a substitute object for IMessageEncryptor stored in the scenario as " +
                    "MessageEncryptorTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 12
 testRunner.And("there exists a BasicConsumableDummy stored in the scenario as BasicConsumableDumm" +
                    "yTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.And("the scenario object BasicConsumableDummy BasicConsumableDummyTest is not expired", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 14
 testRunner.And("the ToObject method on IObjectSerializer substitute ObjectSerializerTest returns " +
                    "BasicConsumableDummyTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 15
 testRunner.And(@"there exists a BasicConsumerWrapperDummy stored in the scenario as BasicConsumerWrapperDummyTest with CommonConnection CommonConnectionTest, ExchangeNameProvider ExchangeNameProviderTest, ConsumerFactory ConsumerFactoryTest, ObjectSerializer ObjectSerializerTest and MessageEncryptor MessageEncryptorTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("HandleBasicDeliver should relay message")]
        public virtual void HandleBasicDeliverShouldRelayMessage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("HandleBasicDeliver should relay message", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 18
 testRunner.Given("the scenario object BasicConsumableDummy BasicConsumableDummyTest is not expired", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
 testRunner.When("the connection is established on ICommonConnection CommonConnectionTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
 testRunner.When("the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDu" +
                    "mmyTest is called", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 21
 testRunner.Then("the method Consume on IBasicConsumer<BasicConsumableDummy> BasicConsumerTest is c" +
                    "alled", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
 testRunner.Then("the method BasicAck on the CommonModel of BasicConsumerWrapperDummy BasicConsumer" +
                    "WrapperDummyTest is called", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("HandleBasicDeliver should not relay expired message")]
        public virtual void HandleBasicDeliverShouldNotRelayExpiredMessage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("HandleBasicDeliver should not relay expired message", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 25
 testRunner.Given("the scenario object BasicConsumableDummy BasicConsumableDummyTest is expired", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 26
 testRunner.And("the scenario object BasicConsumableDummy BasicConsumableDummyTest should not be r" +
                    "elayed if it is expired", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.When("the connection is established on ICommonConnection CommonConnectionTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 28
 testRunner.When("the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDu" +
                    "mmyTest is called", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 29
 testRunner.Then("the method Consume on IBasicConsumer<BasicConsumableDummy> BasicConsumerTest is n" +
                    "ot called", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 30
 testRunner.Then("the method BasicNack on the CommonModel of BasicConsumerWrapperDummy BasicConsume" +
                    "rWrapperDummyTest is called", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("HandleBasicDeliver should throw away non parsable message")]
        public virtual void HandleBasicDeliverShouldThrowAwayNonParsableMessage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("HandleBasicDeliver should throw away non parsable message", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 4
this.FeatureBackground();
#line 33
 testRunner.Given("the ToObject method on IObjectSerializer substitute ObjectSerializerTest returns " +
                    "corrupted message", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 34
 testRunner.When("the connection is established on ICommonConnection CommonConnectionTest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 35
 testRunner.When("the method HandleBasicDeliver on BasicConsumerWrapperDummy BasicConsumerWrapperDu" +
                    "mmyTest is called", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 36
 testRunner.Then("the method Consume on IBasicConsumer<BasicConsumableDummy> BasicConsumerTest is n" +
                    "ot called", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 37
 testRunner.Then("the method BasicNack on the CommonModel of BasicConsumerWrapperDummy BasicConsume" +
                    "rWrapperDummyTest is called", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
