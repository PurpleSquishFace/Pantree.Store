using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pantree.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantree.Helpers.Tests
{
    [TestClass()]
    public class CompressionTests
    {
        [TestMethod()]
        public void CompressTest()
        {
            // Assign
            var input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.In eleifend elit quis arcu sagittis, non scelerisque purus eleifend.Integer a laoreet lacus. In pharetra ligula vitae bibendum tempor. Curabitur eu mattis nibh. Sed eu enim velit. Vivamus luctus dignissim gravida. Maecenas vulputate purus at feugiat eleifend. Sed varius tortor nec diam sagittis, in eleifend orci vehicula.Fusce id justo lacus. Aliquam et purus odio.";

            // Act
            var output = Compression.Compress(input);

            // Assert
            Assert.IsTrue(input.Length > output.Length);
        }

        [TestMethod()]
        public void DecompressTest()
        {
            // Assign
            var input = "H4sIAAAAAAAACk2QUWoDMQxErzIHKHuHUigU2q9C/7VeZTNlbW8sac9fJSUhYBiwpNHTfPahFdwtKpa+9QGjQ6r6C0pvpsXVY0AW7rTCtkI3+vTRUpUnbcvtA5egQUYJmKx0p72g9QYr2Tdol1DsMcIec+nhump6Y5PEUE8tYRPSfD/LUB9Z4hqb4KCLYuacg4nqWvc+JrzFkJlXQA1Uua5F43ye8K0JFtDGiuOGjB8eUhNgi+IpC9dGs6yvQw4uMuFLtGgTwxHbHi5+ZxbHSWNl6gP/tuKQwax7H/nQtKSt1KcM+BRUH4UJc2bJk6b3yGzABb9h3u+3v268RDpkGv+r+8I+/QHx+fxqpwEAAA==";

            // Act
            var output = Compression.Decompress(input);

            // Assert
            Assert.AreEqual(output, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.In eleifend elit quis arcu sagittis, non scelerisque purus eleifend.Integer a laoreet lacus. In pharetra ligula vitae bibendum tempor. Curabitur eu mattis nibh. Sed eu enim velit. Vivamus luctus dignissim gravida. Maecenas vulputate purus at feugiat eleifend. Sed varius tortor nec diam sagittis, in eleifend orci vehicula.Fusce id justo lacus. Aliquam et purus odio.");
        }

        [TestMethod()]
        public void CompressAndDecompressTest()
        {
            // Assign
            var input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam efficitur aliquam odio, non efficitur ex semper at. Vestibulum pretium leo ac urna pretium, non hendrerit justo auctor. Suspendisse sem nisl, rutrum vitae commodo vel, malesuada non massa. Sed lacinia egestas magna, in sodales velit tincidunt sit amet. Duis pharetra nec ex sollicitudin vestibulum. Pellentesque at consequat arcu. Vestibulum scelerisque, metus at dapibus cursus, ligula lorem dictum nisi, ut aliquet nisi leo nec orci. Vivamus vestibulum diam eu venenatis sollicitudin. Nullam eleifend dictum quam, nec egestas metus porttitor ut. Fusce gravida ultricies finibus. Donec semper, lorem eget lobortis rhoncus, nisi odio interdum mauris, vel semper massa turpis ut nibh. Vivamus in consectetur nisi, eu sollicitudin lacus. Aliquam scelerisque lobortis lacus ut condimentum. Donec vel porttitor enim. Nam imperdiet lacinia urna id eleifend. Pellentesque congue ultricies interdum.";

            // Act
            var compressed = Compression.Compress(input);
            var output = Compression.Decompress(compressed);

            // Assert
            Assert.AreEqual(input, output);
        }

        [TestMethod()]
        public void CompressTest_Empty()
        {
            // Assign
            var input = "";

            // Act
            var output = Compression.Compress(input);

            // Assert
            Assert.AreEqual(output, "");
        }

        [TestMethod()]
        public void DecompressTest_Empty()
        {
            // Assign
            var input = "";

            // Act
            var output = Compression.Decompress(input);

            // Assert
            Assert.AreEqual(output, "");
        }
    }
}