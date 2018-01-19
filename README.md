# Описание

Инструмент для анализа ISBL кода, который служит для выявления ошибок в коде на стадии разработки. По сути является статическим анализатором кода.

![isblcheck](https://user-images.githubusercontent.com/34789335/34760471-637a5ae4-f5f9-11e7-9dbd-2c61cebeba84.png)

* Умеет выгружать разработку из базы данных, из пакета разработки (ISX-файла), а также из папки с разработкой, используемой утилитой [DTU](https://github.com/DirectumCompany/DevelopmentTransferUtility);
* Умеет отображать результаты проверки и подсвечивать ошибки прямо в вычислениях;
* Отчет о проверке отображается в окне утилиты и может быть сохранен в файл;
* Имеется консольный агент для проверки разработки в невизуальном режиме.

Категории ошибок:

* Возможные Runtime ошибки (ERROR);
* Проверки на неоптимальный код (WARNING);
* Прочие проверки (INFO).

# Состав сборок

[**IsblCheck**](https://github.com/DirectumCompany/IsblCheck/tree/master/src/IsblCheck)
GUI версия приложения. Имеет возможность загружать пакеты в проводник, открывать код на просмотр, отправлять код на проверку и выводить ошибки. GUI очень похож на Visual Studio. Есть возможность перемещать панели как хочется пользователю.

[**IsblCheck.Agent**](https://github.com/DirectumCompany/IsblCheck/tree/master/src/IsblCheck.Agent)
Консольная версия IsblCheck, которая проверяет код написанный на ISBL по указанной конфигурации. В конфигурации указывается параметры для загрузки разработки, состав отчетов, сборки с правилами и пр.

[**IsblCheck.Core**](https://github.com/DirectumCompany/IsblCheck/tree/master/src/IsblCheck.Core)
Ядро IsblCheck. Содержит в себе все интерфейсы, менеджеры, лексический и синтаксический парсер. Выполняет непосредственную проверку по правилам.

[**IsblCheck.Context.Development**](https://github.com/DirectumCompany/IsblCheck/tree/master/src/IsblCheck.Context.Development)
Сборка загрузки контекста разработки. Выполняет загрузку прикладного кода.

[**IsblCheck.Context.Application**](https://github.com/DirectumCompany/IsblCheck/tree/master/src/IsblCheck.Context.Application)
Сборка загрузки контекста приложения. Выполняет загрузку системных констант, перечислений, ОМ IS-Builder.

[**IsblCheck.Reports**](https://github.com/DirectumCompany/IsblCheck/tree/master/src/IsblCheck.Reports)
Сборка работы с отчетами. Содержит классы по работе с отчетами. Содержит принтеры отчетов: в консоль, в CSV-файл.

[**IsblCheck.Setup**](https://github.com/DirectumCompany/IsblCheck/tree/master/installer/IsblCheck.Setup)
Standalone-инсталлятор для IsblCheck. Пока без интерфейса, простой msi-пакет.

[**IsblCheck.Agent.Setup**](https://github.com/DirectumCompany/IsblCheck/tree/master/installer/IsblCheck.Agent.Setup)
Standalone-инсталлятор для агента IsblCheck. Пока без интерфейса, простой msi-пакет.

# Сборка

#### Установка необходимого ПО

Для построения проекта необходимо:
* Visual Studio 2015 и выше;
* [Расширение Antlr](https://visualstudiogallery.msdn.microsoft.com/25b991db-befd-441b-b23b-bb5f8d07ee9f)
* [JRE](http://www.oracle.com/technetwork/java/javase/downloads/index.html)

Для сборки инсталляторов дополнительно необходимо поставить:
* WiX Toolset v3;
* расширение WiX для Visual Studio.

Всё это можно найти здесь: http://wixtoolset.org/releases/

#### Порядок сборки

* Скачать проект через утилиты по работе с Git;
* Восстановить зависимости решения через NuGet;
* Выполнить сборку решения. Сборка выполняется в каталог _$(SolutionDir)\artifacts\bin_
