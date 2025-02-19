#region Copyright
////////////////////////////////////////////////////////////////////////////////
// The following FIT Protocol software provided may be used with FIT protocol
// devices only and remains the copyrighted property of Garmin Canada Inc.
// The software is being provided on an "as-is" basis and as an accommodation,
// and therefore all warranties, representations, or guarantees of any kind
// (whether express, implied or statutory) including, without limitation,
// warranties of merchantability, non-infringement, or fitness for a particular
// purpose, are specifically disclaimed.
//
// Copyright 2020 Garmin Canada Inc.
////////////////////////////////////////////////////////////////////////////////
// ****WARNING****  This file is auto-generated!  Do NOT edit this file.
// Profile Version = 21.40Release
// Tag = production/akw/21.40.00-0-g813c158
////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Linq;

namespace Dynastream.Fit
{
    internal interface IValidator
    {
        /// <summary>
        /// Validate if a Message is compatible with a protocol version
        /// </summary>
        /// <param name="mesg">Message to validate</param>
        /// <returns>true if message is compatible. false otherwise</returns>
        bool ValidateMesg(Mesg mesg);

        /// <summary>
        /// Validate if a MessageDefinition is compatible with a protocol version
        /// </summary>
        /// <param name="defn">Definition to validate</param>
        /// <returns>true if definition is compatible. false otherwise</returns>
        bool ValidateMesgDefn(MesgDefinition defn);
    }

    /// <summary>
    /// Validates Protocol Features for a given give version
    /// </summary>
    internal class ProtocolValidator
        : IValidator
    {
        private readonly IValidator m_validator;

        public ProtocolValidator(ProtocolVersion version)
        {
            switch (version)
            {
                case ProtocolVersion.V10:
                    m_validator = new V1Validator();
                    break;

                default:
                    m_validator = null;
                    break;
            }
        }

        /// <summary>
        /// Validate if a Message is compatible with a protocol version
        /// </summary>
        /// <param name="mesg">Message to validate</param>
        /// <returns>true if message is compatible. false otherwise</returns>
        public bool ValidateMesg(Mesg mesg)
        {
            if (m_validator == null)
                return true;

            return m_validator.ValidateMesg(mesg);
        }

        /// <summary>
        /// Validate if a MessageDefinition is compatible with a protocol version
        /// </summary>
        /// <param name="defn">Definition to validate</param>
        /// <returns>true if definition is compatible. false otherwise</returns>
        public bool ValidateMesgDefn(MesgDefinition defn)
        {
            if (m_validator == null)
                return true;

            return m_validator.ValidateMesgDefn(defn);
        }
    } // Class

    internal class V1Validator
        : IValidator
    {
        /// <summary>
        /// Validate if a Message is compatible with a protocol version
        /// </summary>
        /// <param name="mesg">Message to validate</param>
        /// <returns>true if message is compatible. false otherwise</returns>
        public bool ValidateMesg(Mesg mesg)
        {
            if (mesg.DeveloperFields.Any())
            {
                return false;
            }

            foreach (var fld in mesg.Fields)
            {
                int typeNum = fld.Type & Fit.BaseTypeNumMask;

                if (typeNum > Fit.Byte)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validate if a MessageDefinition is compatible with a protocol version
        /// </summary>
        /// <param name="defn">Definition to validate</param>
        /// <returns>true if definition is compatible. false otherwise</returns>
        public bool ValidateMesgDefn(MesgDefinition defn)
        {
            if (defn.DeveloperFieldDefinitions.Any())
            {
                return false;
            }

            foreach (var fld in defn.GetFields())
            {
                int typeNum = fld.Type & Fit.BaseTypeNumMask;

                if (typeNum > Fit.Byte)
                {
                    return false;
                }
            }

            return true;
        }
    }
} // namespace
