

public static class DefsMetadata
{
    // Stage
    static public int MAX_STAGE_NUM_PER_CHAPTER { get; private set; }
    

    static public void InitDefineDatas()
    {
        MAX_STAGE_NUM_PER_CHAPTER = (int)Metadata.Define.GetValue("max_stage_num_per_chapter");
    }
}