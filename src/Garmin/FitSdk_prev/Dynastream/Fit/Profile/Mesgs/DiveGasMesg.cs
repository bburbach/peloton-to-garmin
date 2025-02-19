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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Linq;

namespace Dynastream.Fit
{
    /// <summary>
    /// Implements the DiveGas profile message.
    /// </summary>
    public class DiveGasMesg : Mesg
    {
        #region Fields
        #endregion

        /// <summary>
        /// Field Numbers for <see cref="DiveGasMesg"/>
        /// </summary>
        public sealed class FieldDefNum
        {
            public const byte MessageIndex = 254;
            public const byte HeliumContent = 0;
            public const byte OxygenContent = 1;
            public const byte Status = 2;
            public const byte Invalid = Fit.FieldNumInvalid;
        }

        #region Constructors
        public DiveGasMesg() : base(Profile.GetMesg(MesgNum.DiveGas))
        {
        }

        public DiveGasMesg(Mesg mesg) : base(mesg)
        {
        }
        #endregion // Constructors

        #region Methods
        ///<summary>
        /// Retrieves the MessageIndex field</summary>
        /// <returns>Returns nullable ushort representing the MessageIndex field</returns>
        public ushort? GetMessageIndex()
        {
            Object val = GetFieldValue(254, 0, Fit.SubfieldIndexMainField);
            if(val == null)
            {
                return null;
            }

            return (Convert.ToUInt16(val));
            
        }

        /// <summary>
        /// Set MessageIndex field</summary>
        /// <param name="messageIndex_">Nullable field value to be set</param>
        public void SetMessageIndex(ushort? messageIndex_)
        {
            SetFieldValue(254, 0, messageIndex_, Fit.SubfieldIndexMainField);
        }
        
        ///<summary>
        /// Retrieves the HeliumContent field
        /// Units: percent</summary>
        /// <returns>Returns nullable byte representing the HeliumContent field</returns>
        public byte? GetHeliumContent()
        {
            Object val = GetFieldValue(0, 0, Fit.SubfieldIndexMainField);
            if(val == null)
            {
                return null;
            }

            return (Convert.ToByte(val));
            
        }

        /// <summary>
        /// Set HeliumContent field
        /// Units: percent</summary>
        /// <param name="heliumContent_">Nullable field value to be set</param>
        public void SetHeliumContent(byte? heliumContent_)
        {
            SetFieldValue(0, 0, heliumContent_, Fit.SubfieldIndexMainField);
        }
        
        ///<summary>
        /// Retrieves the OxygenContent field
        /// Units: percent</summary>
        /// <returns>Returns nullable byte representing the OxygenContent field</returns>
        public byte? GetOxygenContent()
        {
            Object val = GetFieldValue(1, 0, Fit.SubfieldIndexMainField);
            if(val == null)
            {
                return null;
            }

            return (Convert.ToByte(val));
            
        }

        /// <summary>
        /// Set OxygenContent field
        /// Units: percent</summary>
        /// <param name="oxygenContent_">Nullable field value to be set</param>
        public void SetOxygenContent(byte? oxygenContent_)
        {
            SetFieldValue(1, 0, oxygenContent_, Fit.SubfieldIndexMainField);
        }
        
        ///<summary>
        /// Retrieves the Status field</summary>
        /// <returns>Returns nullable DiveGasStatus enum representing the Status field</returns>
        public DiveGasStatus? GetStatus()
        {
            object obj = GetFieldValue(2, 0, Fit.SubfieldIndexMainField);
            DiveGasStatus? value = obj == null ? (DiveGasStatus?)null : (DiveGasStatus)obj;
            return value;
        }

        /// <summary>
        /// Set Status field</summary>
        /// <param name="status_">Nullable field value to be set</param>
        public void SetStatus(DiveGasStatus? status_)
        {
            SetFieldValue(2, 0, status_, Fit.SubfieldIndexMainField);
        }
        
        #endregion // Methods
    } // Class
} // namespace
