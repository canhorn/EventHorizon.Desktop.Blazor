using System;
using System.Runtime.Serialization;

namespace EventHorizon.Desktop.Blazor.Client.Configuration
{
    [Serializable]
    public class ConfigurationNotInitialized : Exception
    {
        public ConfigurationNotInitialized() { }
        public ConfigurationNotInitialized(string message) : base(message) { }
        public ConfigurationNotInitialized(string message, Exception inner) : base(message, inner) { }
        protected ConfigurationNotInitialized(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}
