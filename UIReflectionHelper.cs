using Forge.Shared;
using System;
using System.Reflection;

namespace Forge.Reflection
{

    /// <summary>Helps to detemine the UI targets</summary>
    public static class UIReflectionHelper
    {

        /// <summary>Determines whether is object win forms control</summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if is object win forms control; otherwise, <c>false</c>.</returns>
        public static bool IsObjectWinFormsControl(object
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
            obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            bool result = false;
            Type type = obj.GetType()
#if NETCOREAPP3_1_OR_GREATER
                !
#endif
                ;
            while (!result && type != null)
            {
                result = Consts.WINDOWS_FORMS_CONTROL.Equals(type.FullName);
                type = type.BaseType;
            }
            return result;
        }

        /// <summary>Invokes the on win forms control.</summary>
        /// <param name="control">The control.</param>
        /// <param name="d">The d.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static object InvokeOnWinFormsControl(object
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
            control, Delegate d, object[] parameters)
        {
            if (control == null) throw new ArgumentNullException("control");
            MethodInfo miInvoke = control.GetType().GetMethod("Invoke", new Type[] { typeof(Delegate), typeof(object[]) });
            return miInvoke.Invoke(control, new object[] { d, parameters });
        }

        /// <summary>Determines whether object WPF dependency</summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if object WPF dependency; otherwise, <c>false</c>.</returns>
        public static bool IsObjectWPFDependency(object
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
            obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            bool result = false;
            Type type = obj.GetType()
#if NETCOREAPP3_1_OR_GREATER
                !
#endif
                ;
            while (!result && type != null)
            {
                result = Consts.WINDOWS_DEPENDENCY_OBJECT.Equals(type.FullName);
                type = type.BaseType;
            }
            return result;
        }

        /// <summary>Invokes on WPF dependency.</summary>
        /// <param name="control">The control.</param>
        /// <param name="d">The delegate</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>object</returns>
        public static object InvokeOnWPFDependency(object
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
            control, Delegate d, object[] parameters)
        {
            if (control == null) throw new ArgumentNullException("control");
            MethodInfo miDispatcher = control.GetType().GetProperty("Dispatcher").GetGetMethod();
            object dispatcherObj = miDispatcher.Invoke(control, null);
            MethodInfo miInvoke = dispatcherObj.GetType().GetMethod("Invoke", new Type[] { typeof(Delegate), typeof(object[]) });
            return miInvoke.Invoke(dispatcherObj, new object[] { d, parameters });
        }

    }

}
