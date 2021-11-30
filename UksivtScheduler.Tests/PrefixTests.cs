using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UksivtScheduler_PC.Classes.General;

/// <summary>
/// ������� � �������.
/// </summary>
namespace UksivtScheduler.Tests
{
    /// <summary>
    /// �����, ���������� ����� �� ��������� ���������.
    /// </summary>
    [TestClass]
    public class PrefixTests
    {
        /// <summary>
        /// ���������� "20�-4" -> �������� "Programming".
        /// </summary>
        [TestMethod]
        public void GetPrefixFromName_Send20P4_GetProgramming()
        {
            String expected = "Programming";
            String actual = "20�-4".GetPrefixFromName();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ���������� "19���-1" -> �������� "Economy".
        /// </summary>
        [TestMethod]
        public void GetPrefixFromName_Send19ZIO1_GetEconomy()
        {
            String expected = "Economy";
            String actual = "19���-1".GetPrefixFromName();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ���������� "21��-3" -> �������� "General".
        /// </summary>
        [TestMethod]
        public void GetPrefixFromName_Send21PD3_GetGeneral()
        {
            String expected = "General";
            String actual = "21��-3".GetPrefixFromName();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// ���������� "21����-1" -> �������� "Technical".
        /// </summary>
        [TestMethod]
        public void GetPrefixFromName_Send21uKSK1_GetTechnical()
        {
            String expected = "Technical";
            String actual = "21����-1".GetPrefixFromName();

            Assert.AreEqual(expected, actual);
        }
    }
}
