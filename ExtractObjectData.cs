/* *********************************************************************
 * Date: 25 Apr 2012
 * Created by: Zoltan Juhasz
 * E-Mail: forge@jzo.hu
***********************************************************************/

using Forge.Shared;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Forge.Reflection
{

    /// <summary>
    /// Mine data from private field of an object. This class used by the infrastructure.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("[{GetType()}, FieldName = '{mFieldName}', FieldNamePart = '{mFieldNamePart}']")]
    public sealed class ExtractObjectData : IEquatable<ExtractObjectData>
    {

        #region Field(s)

        private readonly string
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
            mFieldName = null;

        private readonly string
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
            mFieldNamePart = null;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Prevents a default instance of the <see cref="ExtractObjectData"/> class from being created.
        /// </summary>
        /// <param name="fieldOrPropertyName">Name of the field or property.</param>
        private ExtractObjectData(String fieldOrPropertyName)
        {
            mFieldName = fieldOrPropertyName;
            if (fieldOrPropertyName.Contains("."))
            {
                mFieldName = fieldOrPropertyName.Substring(0, fieldOrPropertyName.IndexOf("."));
                mFieldNamePart = fieldOrPropertyName.Substring(fieldOrPropertyName.IndexOf(".") + 1, fieldOrPropertyName.Length - fieldOrPropertyName.IndexOf(".") - 1);
            }
        }

        #endregion

        #region Public method(s)

        /// <summary>
        /// Creates the specified field name.
        /// </summary>
        /// <param name="fieldOrPropertyName">Name of the field or property.</param>
        /// <returns></returns>
        public static ExtractObjectData Create(string fieldOrPropertyName)
        {
            return new ExtractObjectData(fieldOrPropertyName);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public object GetValue(object o)
        {
            if (o == null)
            {
                ThrowHelper.ThrowArgumentNullException("o");
            }

            object
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
                result = null;
            try
            {
                FieldInfo field = TypeHelper.GetFieldByName(o, mFieldName);
                if (!string.IsNullOrEmpty(mFieldNamePart))
                {
                    ExtractObjectData subField = Create(mFieldNamePart);
                    result = subField.GetValue(field.GetValue(o));
                }
                else
                {
                    result = field.GetValue(o);
                }
            }
            catch (MissingFieldException)
            {
                bool propNotFound = false;
                try
                {
                    PropertyInfo prop = TypeHelper.GetPropertyByName(o, mFieldName);
                    if (!string.IsNullOrEmpty(mFieldNamePart))
                    {
                        ExtractObjectData subField = Create(mFieldNamePart);
                        result = subField.GetValue(prop.GetGetMethod(true).Invoke(o, null));
                    }
                    else
                    {
                        result = prop.GetGetMethod(true).Invoke(o, null);
                    }
                }
                catch (MissingMemberException)
                {
                    propNotFound = true;
                }
                if (propNotFound)
                {
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
            obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!obj.GetType().Equals(GetType())) return false;

            ExtractObjectData other = (ExtractObjectData)obj;
            return ((other.mFieldName == null ? mFieldName == null : other.mFieldName.Equals(mFieldName))
                        && (other.mFieldNamePart == null ? mFieldNamePart == null : other.mFieldNamePart.Equals(mFieldNamePart)));
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(ExtractObjectData
#if NETCOREAPP3_1_OR_GREATER
            ? 
#endif
            other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(mFieldNamePart))
            {
                result = string.Format("{0}.{1}", mFieldName, mFieldNamePart);
            }
            else
            {
                result = mFieldName;
            }
            return result;
        }

#endregion

    }

}
