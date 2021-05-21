namespace Neuro.Models
{
    /// <summary>
    /// Образ и класс для распознавания нейроном.
    /// </summary>
    class ImageData
    {
        /// <summary>
        /// Образ, представленный в виде вектора.
        /// </summary>
        public float[] Data { get; set; }

        /// <summary>
        /// Индекс класса, к которому принадлежит образ.
        /// </summary>
        public int Class { get; set; }

        /// <summary>
        /// Ожидаемые значения на выходах R-элементов для
        /// изображения установленного класса.
        /// </summary>
        public float[] ClassVector { get; set; }
    }
}
