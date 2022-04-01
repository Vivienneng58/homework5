using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e1
{
    class Menu
    {
        public OrderService OS;
        public Menu()
        {
            OS = new OrderService();
            Console.WriteLine("订单管理系统");
            ShowMenu();
        }
        public void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("菜单");
            Console.WriteLine("1.添加订单");
            Console.WriteLine("2.删除订单");
            Console.WriteLine("3.修改订单");
            Console.WriteLine("4.查询订单");
            Console.WriteLine("5.显示所有订单");
            Console.WriteLine("6.显示菜单");
            Console.WriteLine("7.退出");
            Console.WriteLine();
            Console.WriteLine("输入你的选择[1-7]：");

            int selectNum;
            while (true)
            {
                if(!Int32.TryParse(Console.ReadLine(),out selectNum))
                {
                    Console.WriteLine("输入你的选择[1-7]：");
                    continue;
                }
                switch (selectNum)
                {
                    case 1:
                        long orderID;
                        if(AddOrderByUser(out orderID))
                        {
                            Console.WriteLine($"订单#{orderID}添加成功");
                        }
                        else
                        {
                            Console.WriteLine("订单添加失败");
                        }
                        break;
                    case 2:
                        if (RemoveOrderByUser())
                        {
                            Console.WriteLine("订单删除成功");
                        }
                        else
                        {
                            Console.WriteLine("订单删除失败");
                        }
                        break;
                    case 3:
                        if (ModifyOrderByUser())
                        {
                            Console.WriteLine("订单修改成功");
                        }
                        else
                        {
                            Console.WriteLine("订单保持不变");
                        }
                        break;
                    case 4:
                        if (QueryOrderByUser())
                        {
                            Console.WriteLine("订单查询成功");
                        }
                        else
                        {
                            Console.WriteLine("订单查询失败");
                        }
                        break;
                    case 5:
                        OS.OrderList.ForEach(x => Console.WriteLine(x));
                        break;
                    case 6:
                        ShowMenu();
                        return;
                    case 7:
                        OS.Export("orders.xml");
                        return;
                }
                Console.WriteLine();
                Console.WriteLine("输入你的选择[1-7]：");
            }
        }
        //用户添加订单
        //返回订单号
        public bool AddOrderByUser(out long orderID)
        {
            orderID = 0;
            Order order = new Order();
            //保存输入内容
            string enteredContent;
            Console.WriteLine("请输入顾客的资料（点击‘q’退出）：");
            Console.WriteLine("顾客姓名：");
            while ((enteredContent = Console.ReadLine().Trim()) == "")
            {
                Console.WriteLine("请重新输入");
            }
            if (enteredContent == "q" || enteredContent == "Q")
            {
                return false;
            }
            order.CustomerName = enteredContent;

            Console.WriteLine("地址：");
            while ((enteredContent = Console.ReadLine().Trim()) == "")
            {
                Console.WriteLine("请重新输入");
            }
            if (enteredContent == "q" || enteredContent == "Q")
            {
                return false;
            }
            order.Address = enteredContent;

            //记录添加的订单明细数目
            int changeNum;
            AddOrderItemByUser(order, out changeNum);
            Console.WriteLine($"{changeNum}订单货物已添加");
            OS.AddOrder(order);
            orderID = order.OrderID;
            return true;
        }
        public bool RemoveOrderByUser()
        {
            long orderID;
            string enteredContent;
            Console.WriteLine("请输入你想删除的订单的订单号（点击‘q'退出）：");
            
            Console.WriteLine("订单号：");
            while ((enteredContent = Console.ReadLine().Trim()) == "")
            {
                Console.WriteLine("请重新输入");
            }
            if (enteredContent == "q" || enteredContent == "Q")
            {
                return false;
            }
            
            if(!Int64.TryParse(enteredContent,out orderID))
            {
                Console.WriteLine("Error：格式错误");
                return false;
            }

            try
            {
                OS.RemoveOrder(orderID);
                return true;
            }
            catch(ApplicationException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        //用户修改订单信息
        public bool ModifyOrderByUser()
        {
            Order order;
            long orderID;
            string enteredContent;
            Console.WriteLine("请输入你想修改的订单的订单号（点击'q'退出）：");

            Console.Write("订单号：");
            while ((enteredContent = Console.ReadLine().Trim()) == "")
            {
                Console.WriteLine("请重新输入");
            }
            if (enteredContent == "q" || enteredContent == "Q")
            {
                return false;
            }

            if (!Int64.TryParse(enteredContent, out orderID))
            {
                Console.WriteLine("Error: 无此订单号");
                return false;
            }

            if ((order = OS.QueryByOrderID(orderID)) == null)
            {
                Console.WriteLine("Error: 无此订单");
                return false;
            }
            Console.WriteLine();
            Console.WriteLine("1.修改顾客名");
            Console.WriteLine("2.修改地址");
            Console.WriteLine("3.添加订单货物");
            Console.WriteLine("4.删除订单货物");
            Console.WriteLine("5.退出");
            Console.WriteLine();
            Console.WriteLine("输入你的选择[1-5]：");

            int selectNum;
            int changeNum;      //标记添加和删除的订单明细数目

            while (!Int32.TryParse(Console.ReadLine(), out selectNum) || selectNum < 1 || selectNum > 5)
            {
                Console.WriteLine("输入你的选择[1-5]：");
            }

            switch (selectNum)
            {
                case 1:
                case 2:
                    string modifiedContent;
                    if (InputModifiedContentByUser(out modifiedContent))
                    {
                        OS.ModifyOrder(order, selectNum, modifiedContent);
                        return true;
                    }
                    break;
                case 3:
                    AddOrderItemByUser(order, out changeNum);
                    Console.WriteLine($"{changeNum}订单货物已添加");
                    //修改了一条及以上订单明细
                    if (changeNum > 0)
                    {
                        return true;
                    }
                    break;
                case 4:
                    RemoveOrderItemByUser(order, out changeNum);
                    Console.WriteLine($"{changeNum}订单货物已删除");
                    if (changeNum > 0)
                    {
                        return true;
                    }
                    break;
                case 5:
                    break;
            }
            return false;
        }

        //用户查询订单信息
        public bool QueryOrderByUser()
        {
            Console.WriteLine();
            Console.WriteLine("1.查询订单号");
            Console.WriteLine("2.查询货物名");
            Console.WriteLine("3.查询顾客名");
            Console.WriteLine("4.退出");
            Console.WriteLine();
            Console.WriteLine("输入你的选择[1-4]：");

            int selectNum;

            while (!Int32.TryParse(Console.ReadLine(), out selectNum) || selectNum < 1 || selectNum > 4)
            {
                Console.WriteLine("输入你的选择[1-4]：");
            }

            //保存返回的查询结果
            Order res;
            List<Order> listRes;

            switch (selectNum)
            {
                case 1:
                    long orderID;
                    if (!InputQueryContentByUser(out orderID))
                    {
                        return false;
                    }
                    res = OS.QueryByOrderID(orderID);

                    Console.WriteLine("查询结果：");

                    if (res == null)
                    {
                        Console.WriteLine("无匹配订单");
                    }
                    else
                    {
                        Console.WriteLine(res);
                    }

                    break;
                case 2:
                    string queryContent;
                    if (!InputQueryContentByUser(out queryContent))
                    {
                        return false;
                    }
                    listRes = OS.QueryByProductName(queryContent);

                    Console.WriteLine("查询结果：");

                    if (!listRes.Any())
                    {
                        Console.WriteLine("无匹配订单");
                    }
                    else
                    {
                        listRes.ForEach(x => Console.WriteLine(x));
                    }

                    break;
                case 3:
                    if (!InputQueryContentByUser(out queryContent))
                    {
                        return false;
                    }
                    listRes = OS.QueryByCustomerName(queryContent);

                    Console.WriteLine("查询结果：");

                    if (!listRes.Any())
                    {
                        Console.WriteLine("无匹配订单");
                    }
                    else
                    {
                        listRes.ForEach(x => Console.WriteLine(x));
                    }

                    break;
                case 4:
                    return false;
            }
            return true;
        }

        //用户添加订单明细
        //changeNum: 记录添加的订单明细数目
        public void AddOrderItemByUser(Order order, out int changeNum)
        {
            changeNum = 0;
            string[] itemContent;
            string enteredContent;
            double tmpPrice;
            int tmpAmount;
            Console.WriteLine("添加订单货物 [ 格式：货物名 | 货物价格 | 货物数量 ]：");
            //循环添加订单明细
            while ((enteredContent = Console.ReadLine().Trim()) != "q" && enteredContent != "Q")
            {
                if (enteredContent == "")
                {
                    Console.WriteLine("请重新输入");
                    continue;
                }

                itemContent = enteredContent.Split('|');
                //确保订单明细由三项组成
                if (itemContent.Length != 3)
                {
                    Console.WriteLine("格式错误，请重新输入");
                    continue;
                }

                Array.ForEach(itemContent, x => x.Trim());

                if (itemContent[0] == "")
                {
                    Console.WriteLine("格式错误，请重新输入");
                    continue;
                }

                //单价要大于等于零
                if (itemContent[1] == "" || !Double.TryParse(itemContent[1], out tmpPrice) || tmpPrice < 0)
                {
                    Console.WriteLine("价格小于0，请重新输入");
                    continue;
                }

                //数量要大于零
                if (itemContent[2] == "" || !Int32.TryParse(itemContent[2], out tmpAmount) || tmpAmount <= 0)
                {
                    Console.WriteLine("数量小于等于0，请重新输入");
                    continue;
                }

                OrderItem orderItem = new OrderItem
                {
                    ProductName = itemContent[0],
                    ProductPrice = tmpPrice,
                    ProductAmount = tmpAmount
                };

                try
                {
                    OS.AddOrderItem(order, orderItem);
                    changeNum++;
                }
                catch (ApplicationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        //用户删除订单明细
        //changeNum: 记录删除的订单明细数目
        public void RemoveOrderItemByUser(Order order, out int changeNum)
        {
            changeNum = 0;
            string[] itemContent;
            string enteredContent;
            double tmpPrice;
            int tmpAmount;
            Console.WriteLine("删除订单货物 [ 格式：货物名 | 货物价格 | 货物数量 ]：");
            //循环添加订单明细
            while ((enteredContent = Console.ReadLine().Trim()) != "q" && enteredContent != "Q")
            {
                if (enteredContent == "")
                {
                    Console.WriteLine("请重新输入");
                    continue;
                }

                itemContent = enteredContent.Split('|');
                //确保订单明细由三项组成
                if (itemContent.Length != 3)
                {
                    Console.WriteLine("格式错误，请重新输入");
                    continue;
                }

                Array.ForEach(itemContent, x => x.Trim());

                if (itemContent[0] == "")
                {
                    Console.WriteLine("格式错误，请重新输入");
                    continue;
                }

                //单价要大于等于零
                if (itemContent[1] == "" || !Double.TryParse(itemContent[1], out tmpPrice) || tmpPrice < 0)
                {
                    Console.WriteLine("价格小于0，请重新输入");
                    continue;
                }

                //数量要大于零
                if (itemContent[2] == "" || !Int32.TryParse(itemContent[2], out tmpAmount) || tmpAmount <= 0)
                {
                    Console.WriteLine("数量少于等于0，请重新输入");
                    continue;
                }

                OrderItem orderItem = new OrderItem
                {
                    ProductName = itemContent[0],
                    ProductPrice = tmpPrice,
                    ProductAmount = tmpAmount
                };

                try
                {
                    OS.RemoveOrderItem(order, orderItem);
                    changeNum++;
                }
                catch (ApplicationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        //用户输入修改内容
        public bool InputModifiedContentByUser(out string modifiedContent)
        {
            Console.WriteLine("请输入你想修改的内容（点击‘q’退出）：");

            Console.Write("修改内容：");
            while ((modifiedContent = Console.ReadLine().Trim()) == "")
            {
                Console.WriteLine("请重新输入");
            }
            if (modifiedContent == "q" || modifiedContent == "Q")
            {
                return false;
            }
            return true;
        }

        //用户输入查询内容(订单号)
        public bool InputQueryContentByUser(out long orderID)
        {
            orderID = 0;
            string enteredContent;
            Console.WriteLine("输入你想查询的订单的订单号（点击‘q’退出）：");

            Console.Write("订单号：");
            while ((enteredContent = Console.ReadLine().Trim()) == "")
            {
                Console.WriteLine("请重新输入");
            }
            if (enteredContent == "q" || enteredContent == "Q")
            {
                return false;
            }

            if (!Int64.TryParse(enteredContent, out orderID))
            {
                Console.WriteLine("Error：格式错误");
                return false;
            }
            return true;
        }

        //用户输入查询内容(商品名称和客户等字段)
        public bool InputQueryContentByUser(out string queryContent)
        {
            Console.WriteLine("请输入你想查询的内容（点击‘q’退出）：");

            Console.Write("查询内容：");
            while ((queryContent = Console.ReadLine().Trim()) == "")
            {
                Console.WriteLine("请重新输入");
            }
            if (queryContent == "q" || queryContent == "Q")
            {
                return false;
            }
            return true;
        }
    }
}
