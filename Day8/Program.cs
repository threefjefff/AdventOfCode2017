namespace Day8
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    class Program
    {
        [Flags]
        enum Sign
        {
            Gt = 1,
            Lt = 2,
            Eq = 4,
            Not = 8,
        }
        struct Instruction
        {
            public string TargetReg { get; set; }
            public int ChangeInValue { get; set; }
            public Condition Condition { get; set; }
        }

        struct Condition
        {
            public string Reg { get; set; }
            public Sign Sign { get; set; }
            public int Value { get; set; }
        }
        private static string Input => File.ReadAllText("input.txt");

        private static Instruction[] ParseInstructions()
        {
            var parsedInstructions = new List<Instruction>();
            foreach (Match match in Regex.Matches(Input, @"(\w+)\s(\w+)\s([-0-9]+)\sif\s(\w+)\s([><=!]+)\s([-0-9]+)"))
            {
                parsedInstructions.Add(new Instruction
                {
                    ChangeInValue = int.Parse(match.Groups[3].Value) * (match.Groups[2].Value == "inc" ? 1 : -1),
                    TargetReg = match.Groups[1].Value,
                    Condition = new Condition
                    {
                        Reg = match.Groups[4].Value,
                        Value = int.Parse(match.Groups[6].Value),
                        Sign = ParseSign(match.Groups[5].Value)
                    }
                });
            }
            return parsedInstructions.ToArray();
        }

        static Sign ParseSign(string sign)
        {
            Sign parsed;
            switch (sign[0])
            {
                case '>':
                    parsed = Sign.Gt;
                    parsed = sign.Length > 1 ? parsed | Sign.Eq : parsed;
                    break;
                case '<':
                    parsed = Sign.Lt;
                    parsed = sign.Length > 1 ? parsed | Sign.Eq : parsed;
                    break;
                case '=':
                    parsed = Sign.Eq;
                    break;
                case '!':
                    parsed = Sign.Not;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sign), "Fuckfuckfuck");
            }
            return parsed;
        }

        private static int MaxRuntime = 0;

        static void Main(string[] args)
        {
            var registers = RunInstructions();
            var max = registers.Values.Max();
            Console.WriteLine(max);
            Console.WriteLine(MaxRuntime);
            Console.Read();
        }

        static Dictionary<string, int> RunInstructions()
        {
            var registers = new Dictionary<string, int>();
            var instructions = ParseInstructions();
            foreach (var instruction in instructions)
            {
                var targetRegVal = FetchReg(registers, instruction.TargetReg);
                if (CalcCondition(registers, instruction.Condition))
                {
                    targetRegVal += instruction.ChangeInValue;
                    MaxRuntime = targetRegVal > MaxRuntime ? targetRegVal : MaxRuntime;
                }
                registers[instruction.TargetReg] = targetRegVal;
            }
            return registers;
        }

        static bool CalcCondition(Dictionary<string, int> registers, Condition condition)
        {
            var conditionRegValue = FetchReg(registers, condition.Reg);
            bool? passes = null;
            if (condition.Sign.HasFlag(Sign.Not))
            {
                passes = condition.Value != conditionRegValue;
            }
            if (condition.Sign.HasFlag(Sign.Gt))
            {
                passes = conditionRegValue > condition.Value;
            }
            if (condition.Sign.HasFlag(Sign.Lt))
            {
                passes = conditionRegValue < condition.Value;
            }
            if (condition.Sign.HasFlag(Sign.Eq))
            {
                var equal = condition.Value == conditionRegValue;
                passes = passes == null ? equal : equal || passes.Value;
            }
            if (passes == null)
            {
                throw new ArgumentOutOfRangeException("I don't know how this happened");
            }
            return passes.Value;
        }

        static int FetchReg(Dictionary<string, int> registers, string register)
        {
            if (!registers.TryGetValue(register, out int regValue))
            {
                registers.Add(register, 0);
            }
            return regValue;
        }
    }
}
