using GestureRecognizer;


public class InterviewDrawmechanic : DrawMechanic
{
    public override void StopDrawing()
    {
       BrightenLines();
       _audio.Stop();
       
       InterviewEvents.InvokeOnSignDone();
       
       print("interview stopdrawing");
    }
    
    
}
