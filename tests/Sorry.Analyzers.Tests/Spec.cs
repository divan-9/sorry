namespace Sorry.Analyzers.Tests
{
    public static class Spec
    {
        public const string Trait = "spec";

        public static class SP1879
        {
            public const string UserVoice1 = @"
            Я, разработчик RetailRocket, хочу запретить использование primary constructor,
            чтобы инициализация экземпляра была единообразной";

            public const string UserVoice2 = @"
            Я, разработчик RetailRocket, хочу запретить использование primary constructor,
            чтобы упростить навигацию по кодовой базе";

            public const string UserVoice3 = @"
            Я, разработчик RetailRocket, хочу запретить использование primary constructor,
            чтобы при чтении кода метода класса понимать идет ли речь о параметре метода
            или о поле экземпляра";

            public const string R1 = @"
                Если в коде класса используется primary constructor, то будет ошибка анализа RRCA0001 этого кода";

            public const string R2 = @"
                Если в коде класса нет никакого конструктора, то не будет ошибки анализа этого кода";
        }
    }
}