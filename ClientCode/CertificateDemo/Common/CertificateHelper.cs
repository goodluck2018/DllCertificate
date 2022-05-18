using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CertificateDemo
{
    public class CertificateHelper
    {
        public class AssemblyDynamicLoader<T>
        {
            private AppDomain appDomain;

            private DynamicRemoteLoadAssembly<T> remoteLoader;

            public T InvokeMethod(string assemblyName, string assemblyPath, string assemblyConfigFilePath, string fullClassName, string methodName, params object[] args)
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationName = "ApplicationLoader";
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory + @"bin\";
                //setup.PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "private");
                setup.CachePath = setup.ApplicationBase;
                setup.ShadowCopyFiles = "true";
                if (assemblyConfigFilePath != string.Empty)
                {
                    setup.ConfigurationFile = AppDomain.CurrentDomain.BaseDirectory + assemblyConfigFilePath;
                }
                setup.ShadowCopyDirectories = setup.ApplicationBase;
                setup.LoaderOptimization = LoaderOptimization.SingleDomain;

                this.appDomain = AppDomain.CreateDomain("ApplicationLoaderDomain", null, setup);
                String name = Assembly.GetExecutingAssembly().GetName().FullName;

                this.remoteLoader = (DynamicRemoteLoadAssembly<T>)this.appDomain.CreateInstanceAndUnwrap(name, typeof(DynamicRemoteLoadAssembly<T>).FullName);

                assemblyName = AppDomain.CurrentDomain.BaseDirectory + assemblyPath + assemblyName;

                return this.remoteLoader.InvokeMethod(assemblyName, fullClassName, methodName, args);
            }

            /// <summary>
            ///
            /// </summary>
            public void Unload()
            {
                try
                {
                    AppDomain.Unload(this.appDomain);
                    this.appDomain = null;
                }
                catch (CannotUnloadAppDomainException ex)
                {

                }
            }
        }

        public class DynamicRemoteLoadAssembly<T> : MarshalByRefObject
        {
            private Assembly assembly = null;

            public T InvokeMethod(string assemblyPath, string fullClassName, string methodName, params object[] args)
            {
                this.assembly = null;
                T result = default(T);
                try
                {
                    this.assembly = Assembly.LoadFile(assemblyPath);
                    Type pgmType = null;
                    if (this.assembly != null)
                    {
                        pgmType = this.assembly.GetType(fullClassName, true, true);
                    }
                    else
                    {
                        pgmType = Type.GetType(fullClassName, true, true);
                    }
                    BindingFlags defaultBinding = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.InvokeMethod | BindingFlags.Static;
                    CultureInfo cultureInfo = new CultureInfo("es-ES", false);
                    try
                    {
                        MethodInfo methisInfo = assembly.GetType(fullClassName, true, true).GetMethod(methodName);

                        if (methisInfo == null)
                        {
                            new Exception("EMethod　does　not　exist!");
                        }

                        if (methisInfo.IsStatic)
                        {
                            if (methisInfo.GetParameters().Length == 0)
                            {
                                if (methisInfo.ReturnType == typeof(void))
                                {
                                    pgmType.InvokeMember(methodName, defaultBinding, null, null, null, cultureInfo);
                                }
                                else
                                {
                                    result = (T)pgmType.InvokeMember(methodName, defaultBinding, null, null, null, cultureInfo);
                                }
                            }
                            else
                            {
                                if (methisInfo.ReturnType == typeof(void))
                                {
                                    pgmType.InvokeMember(methodName, defaultBinding, null, null, args, cultureInfo);
                                }

                                else
                                {
                                    result = (T)pgmType.InvokeMember(methodName, defaultBinding, null, null, args, cultureInfo);
                                }
                            }
                        }
                        else
                        {

                            if (methisInfo.GetParameters().Length == 0)
                            {
                                object pgmClass = Activator.CreateInstance(pgmType);
                                if (methisInfo.ReturnType == typeof(void))
                                {
                                    pgmType.InvokeMember(methodName, defaultBinding, null, pgmClass, null, cultureInfo);
                                }
                                else
                                {
                                    result = (T)pgmType.InvokeMember(methodName, defaultBinding, null, pgmClass, null, cultureInfo);
                                }
                            }
                            else
                            {
                                object pgmClass = Activator.CreateInstance(pgmType);
                                if (methisInfo.ReturnType == typeof(void))
                                {
                                    pgmType.InvokeMember(methodName, defaultBinding, null, pgmClass, args, cultureInfo);
                                }
                                else
                                {
                                    result = (T)pgmType.InvokeMember(methodName, defaultBinding, null, pgmClass, args, cultureInfo);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        result = (T)pgmType.InvokeMember(methodName, defaultBinding, null, null, null, cultureInfo);
                    }
                    return result;
                }
                catch (Exception ee)
                {
                    return result;
                }
            }
        }
    }
}

 
