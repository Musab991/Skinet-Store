using System.Security.Cryptography.X509Certificates;

namespace Core.Specifications
{
    public class OrderSpecification : BaseSpecification<Core.Entities.OrderAggregate.Order>
    {

        public OrderSpecification(string email):base(x=>x.BuyerEmail==email)
        {

            Includes.Add(x => x.OrderItems);
            Includes.Add(x => x.DeliveryMethod);
            AddOrderByDescending(x => x.OrderDate);
            
        }
        public OrderSpecification(string email,int id) : 
            base(x => x.BuyerEmail == email&&x.Id==id)
        {
            

            IncludeStrings.Add("OrderItems");
            IncludeStrings.Add("DeliveryMethod");
            AddOrderByDescending(x => x.OrderDate);


        }

    }


}
