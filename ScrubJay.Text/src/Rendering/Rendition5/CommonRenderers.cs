//namespace ScrubJay.Text.Rendering;
//
//[PublicAPI]
//public static  class CommonRenderers
//{

//
//    [RenderToMethod<Array>]
//    public static void RenderArrayTo(Array array, TextBuilder builder)
//    {
//        int rank = array.Rank;
//        int[] indices = new int[rank];
//        Array.Clear(indices, 0, indices.Length);
//        iterateDimension(0);
//        return;
//
//
//        void iterateDimension(int dim)
//        {
//            int start = array.GetLowerBound(dim);
//            int end = array.GetUpperBound(dim);
//
//            if (end < start)
//            {
//                builder.Write("[]");
//                return;
//            }
//
//            // any dimension other than the last
//            if (dim < (rank - 1))
//            {
//                // bracket all my children
//                builder.Write('[');
//
//                // iterate them
//                for (var i = start; i <= end; i++)
//                {
//                    indices[dim] = i;
//                    iterateDimension(dim + 1);
//                }
//
//                builder.Write(']');
//                return;
//            }
//
//            // last dimension, we actually write values
//            builder.Write('[');
//            indices[dim] = start;
//            object? obj = array.GetValue(indices);
//            builder.Render(obj);
//            for (var i = start + 1; i <= end; i++)
//            {
//                indices[dim] = i;
//                builder.Write(", ");
//                obj = array.GetValue(indices);
//                builder.Render(obj);
//            }
//            builder.Write(']');
//        }
//    }
//

//
//}