namespace DemoApplication.UserEvent
{
    public interface IProcode
    {
        public bool CheckDuplicate();
    }

    public class Procode: IProcode
    {
        public bool CheckDuplicate(){
            return false;
        }        
    }
}