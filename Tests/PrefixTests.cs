using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UksivtScheduler_PC.Classes.General;

/// <summary>
/// Область с тестами.
/// </summary>
namespace UksivtScheduler.Tests
{
    /// <summary>
    /// Класс, содержащий тесты по получению префиксов.
    /// </summary>
    [TestClass]
    public class PrefixTests
    {
        /// <summary>
        /// Отправлено "20П-4" -> Получено "Programming".
        /// </summary>
        [TestMethod]
        public void GetPrefixFromName_Send20P4_GetProgramming()
        {
            String expected = "Programming";
            String actual = "20П-4".GetPrefixFromName();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Отправлено "19ЗИО-1" -> Получено "Economy".
        /// </summary>
        [TestMethod]
        public void GetPrefixFromName_Send19ZIO1_GetEconomy()
        {
            String expected = "Economy";
            String actual = "19ЗИО-1".GetPrefixFromName();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Отправлено "21ПД-3" -> Получено "General".
        /// </summary>
        [TestMethod]
        public void GetPrefixFromName_Send21PD3_GetGeneral()
        {
            String expected = "General";
            String actual = "21ПД-3".GetPrefixFromName();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Отправлено "21уКСК-1" -> Получено "Technical".
        /// </summary>
        [TestMethod]
        public void GetPrefixFromName_Send21uKSK1_GetTechnical()
        {
            String expected = "Technical";
            String actual = "21уКСК-1".GetPrefixFromName();

            Assert.AreEqual(expected, actual);
        }
    }
}
