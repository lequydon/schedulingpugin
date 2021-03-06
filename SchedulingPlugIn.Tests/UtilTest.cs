// <copyright file="UtilTest.cs" company="Microsoft">Copyright © Microsoft 2019</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchedulingPlugIn.Helper;

namespace SchedulingPlugIn.Helper.Tests
{
    /// <summary>This class contains parameterized unit tests for Util</summary>
    [PexClass(typeof(Util))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class UtilTest
    {
        /// <summary>Test stub for GenerateCode(Int32)</summary>
        [PexMethod]
        public string GenerateCodeTest(int length)
        {
            string result = Util.GenerateCode(length);
            return result;
            // TODO: add assertions to method UtilTest.GenerateCodeTest(Int32)
        }
    }
}
