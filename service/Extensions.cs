namespace FirstServer.service;

public static class Extensions
{
    public const string AudioExt = "mp3";
    
    public static T[] Slice<T>(this T[] source, int index, int length)
    {
        if (index < 0 || length < 0)
        {
            throw new ArgumentOutOfRangeException("index ou length est inférieur à 0.");
        }
        if (source == null)
        {
            throw new ArgumentNullException("source est null.");
        }
        if (index + length > source.Length)
        {
            throw new ArgumentException("La somme de index et length est plus grande que la longueur du tableau source.");
        }

        T[] slice = new T[length];
        Array.Copy(source, index, slice, 0, length);
        return slice;
    }
}