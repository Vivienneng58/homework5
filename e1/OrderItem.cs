using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e1
{
    [Serializable]
    public class OrderItem
    {
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductAmount { get; set; }

        //重写Equals方法比较订单明细不使其重复
        public override bool Equals(object obj)
        {
            var item = obj as OrderItem;
            return item != null &&
                   ProductName == item.ProductName &&
                   ProductPrice == item.ProductPrice &&
                   ProductAmount == item.ProductAmount;
        }

        public override int GetHashCode()
        {
            var hashCode = 525988058;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProductName);
            hashCode = hashCode * -1521134295 + ProductPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + ProductAmount.GetHashCode();
            return hashCode;
        }

        public override string ToString() =>
            $"货物名：\t{ProductName}\n货物价格：\tCNY￥{ProductPrice}\n货物数量：{ProductAmount}";
    }
}
