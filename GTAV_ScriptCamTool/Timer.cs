using GTA;

public class Timer
{
    #region Public Variables
    private bool enabled;
    public bool Enabled
    {
        get { return enabled; }
        set { enabled = value; }
    }

    private int interval;
    public int Interval
    {
        get { return interval; }
        set { this.interval = value; }
    }
    #endregion

    private int waiter;
    public int Waiter
    {
        get { return waiter; }
        set { this.waiter = value; }
    }

    public Timer(int interval)
    {
        this.interval = interval;
        this.waiter = 0;
        this.enabled = false;
    }

    public Timer()
    {
        this.interval = 0;
        this.waiter = 0;
        this.enabled = false;
    }

    public void Start()
    {
        this.waiter = Game.GameTime + interval;
        this.enabled = true;
    }

    public void Reset()
    {
        this.waiter = Game.GameTime + interval;
    }

}
