using System.Collections.Generic;
using System;
using UnityEngine;

namespace CmdConsole
{
    public static class CmdUtility
    {
        public static T ChangeType<T>(this object obj)
        {
            return (T)System.Convert.ChangeType(obj, typeof(T));
        }
        public static bool CanChangeType(this object obj, Type type)
        {
            try
            {
                System.Convert.ChangeType(obj, type);
                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}
