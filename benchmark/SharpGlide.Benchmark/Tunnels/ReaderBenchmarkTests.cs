namespace SharpGlide.Benchmark.Tunnels
{
    // public class ReaderBenchmarkTests
    // {
    //     private const int N = 10000;
    //
    //     // private readonly ReaderDirect _readerDirect = new ReaderDirect();
    //     // private readonly ReaderViaCallback _readerViaCallback = new ReaderViaCallback();
    //
    //     public static void PopulateData(Queue<byte> target)
    //     {
    //         var bytes = new byte[N];
    //         new Random(23).NextBytes(bytes);
    //
    //         foreach (var bt in bytes)
    //         {
    //             target.Enqueue(bt);
    //         }
    //     }
    //
    //     [Benchmark]
    //     public void ReaderDirectBenchmark()
    //     {
    //         PopulateData(this._readerDirect.Store);
    //         var doRead = _readerDirect.ReadExpr.Compile();
    //         var result = new List<byte>();
    //
    //         while (true)
    //         {
    //             try
    //             {
    //                 var data = doRead();
    //                 result.Add(data);
    //             }
    //             catch (NoDataException)
    //             {
    //                 break;
    //             }
    //         }
    //     }
    //     
    //     [Benchmark]
    //     public void ReaderViaCallbackBenchmark()
    //     {
    //         PopulateData(this._readerViaCallback.Store);
    //         var result = new List<byte>();
    //         var doRead = _readerViaCallback.ReadViaCallbackExpr.Compile();
    //         try
    //         {
    //             doRead((data) =>
    //             {
    //                 result.Add(data);      
    //             });
    //         }
    //         catch (NoDataException)
    //         {
    //         }
    //     }
    //     
    //     // public class ReaderDirect : IDirectReader<byte>
    //     // {
    //     //     public bool CanExecute { get; set; } = true;
    //     //
    //     //     public readonly Queue<byte> Store = new ();
    //     //
    //     //     public Expression<Func<byte>> ReadExpr => () => ReadLogic();
    //     //     public Expression<Func<Task<byte>>> ReadAsyncExpr { get; }
    //     //
    //     //     private byte ReadLogic()
    //     //     {
    //     //         if (Store.TryDequeue(out byte data))
    //     //         {
    //     //             return data;
    //     //         }
    //     //
    //     //         throw new NoDataException();
    //     //     }
    //     //
    //     //     public Expression<Func<IEnumerable<byte>>> ReadRangeExpr => throw new NotImplementedException();
    //     // }
    //     //
    //     // public class ReaderViaCallback : IReaderViaCallback<byte>
    //     // {
    //     //     public bool CanExecute { get; set; } = true;
    //     //     public Expression<Action<Action<byte>>> ReadViaCallbackExpr => onReceive => ReadLogic(onReceive);
    //     //     public readonly Queue<byte> Store = new ();
    //     //     
    //     //     private void ReadLogic(Action<byte> onReceive)
    //     //     {
    //     //         while (Store.Count > 0)
    //     //         {
    //     //             if (Store.TryDequeue(out byte data))
    //     //             {
    //     //                 onReceive(data);
    //     //             }
    //     //         }
    //     //
    //     //         throw new NoDataException();
    //     //     }
    //     //
    //     //     public Expression<Action<Action<IEnumerable<byte>>>> ReadRangeViaCallbackExpr =>
    //     //         throw new NotImplementedException();
    //     // }
    // }
}