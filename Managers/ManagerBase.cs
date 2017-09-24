using System;

namespace Managers
{
    public abstract class ManagerBase<T> : IDisposable where T : ManagerBase<T>
    {
        private static Lazy<T> _Instance = new Lazy<T>();
        public static T Instance
        {
            get
            {
                return _Instance.Value;
            }
        }

        protected bool IsInitialized { get; set; }

        public virtual void Initialize()
        {
            if (IsInitialized)
            {
                throw new Exception("Initialize fonksiyonu ikinci kez çalıştırılamaz");
            }
            IsInitialized = true;
        }

        public virtual void Dispose()
        {

        }
    }
}
