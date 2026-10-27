namespace MO32_2_Kulchitskiy_neuronochka.NeuroNet
{
    enum MemoryMode//режимы памяти
    {
        GET, // считывание
        SET, // сохранение
        INIT // инициализация
    }

    enum NeuronType // тип нейрона
    {
        Hidden,
        Output
    }

    enum NetworkMode // режимы работы нейрона
    {
        Train, // обучение
        Test,  // проверка
        Demo   // распознавание
    }
}
