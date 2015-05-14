//using SimpleInjector;

//public class IoC
//{
//    private static Container _Instance;
//    private static readonly object _Locker = new object();
//    public static Container Instance
//    {
//        get
//        {
//            if (_Instance != null) 
//                return _Instance;

//            lock (_Locker)
//            {
//                _Instance = new Container();
//                _Instance.Options.AllowOverridingRegistrations = true;
//            }

//            return _Instance;
//        }
//    }

//    public static void ResetEverything()
//    {
//            _Instance = null;
//    }
//}
