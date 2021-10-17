using System.Collections.Generic;

public class PropertyNotification<T> where T : struct
{
    T data = default(T);
    List<Notification> propertyList = new List<Notification>();
    public delegate void Notification(T _data);
    public event Notification Changed
    {
        add
        {
            if(!propertyList.Contains(value))
                propertyList.Add(value);
        }
        remove
        {
            propertyList.Remove(value);
        }
    }

    public T Value 
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
            NotificationAll();
        }
    }

    private void NotificationAll(){

        for (int i = 0; i < propertyList.Count; i++){
            propertyList[i](data);
        }

        

        // foreach(var property in propertyList)
        //     property?.Invoke(data);
    }

    public void RemoveAll(){
        propertyList.Clear();               
    }
}
