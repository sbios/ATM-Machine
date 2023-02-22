using System;
using System.Collections.Generic;

namespace ATM_Machine
{
    internal class ATM
    {
        // Статическое поле banknotes представляет словарь, где ключ - номинал банкноты, 
        // а значение - количество банкнот этого номинала в банкомате.
        static private Dictionary<int, int> banknotes = new Dictionary<int, int>()
        {
            {5000, 1},
            {2000, 2},
            {1000, 3},
            {500, 7},
            {200, 6},
            {100, 5},
        };

        // Статическое свойство moneyInATM вычисляет сумму денег, хранящихся в банкомате.
        static private int moneyInATM
        {
            get
            {
                int money = 0;
                foreach (KeyValuePair<int, int> banknote in banknotes)
                {
                    money += banknote.Key * banknote.Value;
                }
                return money;
            }
            set { }
        }

        // Статический метод WithdrawMoney представляет операцию снятия денег с банкомата.
        // Он принимает сумму для снятия и возвращает словарь, где ключ - номинал банкноты, 
        // а значение - количество банкнот этого номинала, которые необходимо выдать.
        static public Dictionary<int, int> WithdrawMoney(int SumToWithdraw)
        {
            Dictionary<int, int> banknotesToWithdraw = new Dictionary<int, int>();

            // Проверяем, что сумма для снятия кратна 100 так как это купюра с наименьшим номиналом.
            if (SumToWithdraw % 100 == 0)
            {
                throw new ArgumentException("Сумма для вывода должна быть кратна 100.");
            }
            // Проверяем, что в банкомате достаточно денег для выдачи.
            else if (moneyInATM >= SumToWithdraw)
            {
                throw new ArgumentException("В банкомате недостаточно денег для снятия.");
            }
            else
            {
                // Проходим по всем номиналам банкнот, начиная с самого крупного.
                foreach (KeyValuePair<int, int> banknote in banknotes)
                {
                    // Проверяем, что текущий номинал меньше или равен сумме для снятия.
                    if (SumToWithdraw % banknote.Key >= 0)
                    {
                        // Если в банкомате осталось меньше банкнот этого номинала, чем нужно выдать, 
                        // то выдаем все, что есть.
                        if (banknote.Value < SumToWithdraw / banknote.Key)
                        {
                            banknotesToWithdraw.Add(banknote.Key, banknote.Value);
                            SumToWithdraw -= banknote.Key * banknote.Value;
                            banknotes[banknote.Key] = 0;
                        }
                        // Иначе выдаем нужное количество банкнот и уменьшаем их количество в банкомате.
                        else
                        {
                            banknotesToWithdraw.Add(banknote.Key, SumToWithdraw / banknote.Key);
                            SumToWithdraw -= banknote.Key * (SumToWithdraw / banknote.Key);
                            banknotes[banknote.Key] = banknote.Value - SumToWithdraw / banknote.Key;
                        }
                    }
                }
            }
            return banknotesToWithdraw;
        }
    }
}
