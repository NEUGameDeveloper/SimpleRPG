// Animancer // Copyright 2020 Kybernetik //
// This file has been modified for use in Animancer. The original copyright notice is as follows:
/*
 * Created by C.J. Kimberlin
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2019
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * 
 * TERMS OF USE - EASING EQUATIONS
 * Open source under the BSD License.
 * Copyright (c)2001 Robert Penner
 * All rights reserved.
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE 
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using UnityEngine;

namespace Animancer
{
    /// <summary>Utility functions for various types of interpolation.</summary>
    public static class Interpolation
    {
        /************************************************************************************************************************/
        #region Delegates
        /************************************************************************************************************************/

        /// <summary>
        /// The name of an interpolation function.
        /// The <see cref="Interpolation"/> class contains various extension methods for this enum.
        /// </summary>
        public enum Function
        {
            Linear,
            Spring,

            QuadraticIn,
            QuadraticOut,
            QuadraticInOut,

            CubicIn,
            CubicOut,
            CubicInOut,

            QuarticIn,
            QuarticOut,
            QuarticInOut,

            QuinticIn,
            QuinticOut,
            QuinticInOut,

            SineIn,
            SineOut,
            SineInOut,

            ExponentialIn,
            ExponentialOut,
            ExponentialInOut,

            CircularIn,
            CircularOut,
            CircularInOut,

            BounceIn,
            BounceOut,
            BounceInOut,

            BackIn,
            BackOut,
            BackInOut,

            ElasticIn,
            ElasticOut,
            ElasticInOut,
        }

        private const int FunctionCount = (int)Function.ElasticInOut + 1;

        /************************************************************************************************************************/

        /// <summary>A delegate representing an interpolation function.</summary>
        public delegate float Delegate(float start, float end, float value);

        /************************************************************************************************************************/

        private static Delegate[] _Delegates;

        /// <summary>Returns a cached delegate representing the specified `function`.</summary>
        public static Delegate GetDelegate(this Function function)
        {
            var i = (int)function;
            Delegate del;

            if (_Delegates == null)
            {
                _Delegates = new Delegate[FunctionCount];
            }
            else
            {
                del = _Delegates[i];
                if (del != null)
                    return del;
            }

            switch (function)
            {
                case Function.Linear: del = Linear; break;
                case Function.Spring: del = Spring; break;
                case Function.QuadraticIn: del = QuadraticIn; break;
                case Function.QuadraticOut: del = QuadraticOut; break;
                case Function.QuadraticInOut: del = QuadraticInOut; break;
                case Function.CubicIn: del = CubicIn; break;
                case Function.CubicOut: del = CubicOut; break;
                case Function.CubicInOut: del = CubicInOut; break;
                case Function.QuarticIn: del = QuarticIn; break;
                case Function.QuarticOut: del = QuarticOut; break;
                case Function.QuarticInOut: del = QuarticInOut; break;
                case Function.QuinticIn: del = QuinticIn; break;
                case Function.QuinticOut: del = QuinticOut; break;
                case Function.QuinticInOut: del = QuinticInOut; break;
                case Function.SineIn: del = SineIn; break;
                case Function.SineOut: del = SineOut; break;
                case Function.SineInOut: del = SineInOut; break;
                case Function.ExponentialIn: del = ExponentialIn; break;
                case Function.ExponentialOut: del = ExponentialOut; break;
                case Function.ExponentialInOut: del = ExponentialInOut; break;
                case Function.CircularIn: del = CircularIn; break;
                case Function.CircularOut: del = CircularOut; break;
                case Function.CircularInOut: del = CircularInOut; break;
                case Function.BounceIn: del = BounceIn; break;
                case Function.BounceOut: del = BounceOut; break;
                case Function.BounceInOut: del = BounceInOut; break;
                case Function.BackIn: del = BackIn; break;
                case Function.BackOut: del = BackOut; break;
                case Function.BackInOut: del = BackInOut; break;
                case Function.ElasticIn: del = ElasticIn; break;
                case Function.ElasticOut: del = ElasticOut; break;
                case Function.ElasticInOut: del = ElasticInOut; break;
                default: throw new ArgumentOutOfRangeException(nameof(function));
            }

            _Delegates[i] = del;
            return del;
        }

        /************************************************************************************************************************/

        private static Func<float, float>[] _Delegates01;

        /// <summary>
        /// Returns a cached delegate representing the specified `function` using 0 and 1 as the
        /// `start` and `end` parameters respectively.
        /// </summary>
        public static Func<float, float> GetDelegate01(this Function function)
        {
            var i = (int)function;
            Func<float, float> del;

            if (_Delegates01 == null)
            {
                _Delegates01 = new Func<float, float>[FunctionCount];
            }
            else
            {
                del = _Delegates01[i];
                if (del != null)
                    return del;
            }

            switch (function)
            {
                case Function.Linear: del = Linear01; break;
                case Function.Spring: del = Spring01; break;
                case Function.QuadraticIn: del = QuadraticIn01; break;
                case Function.QuadraticOut: del = QuadraticOut01; break;
                case Function.QuadraticInOut: del = QuadraticInOut01; break;
                case Function.CubicIn: del = CubicIn01; break;
                case Function.CubicOut: del = CubicOut01; break;
                case Function.CubicInOut: del = CubicInOut01; break;
                case Function.QuarticIn: del = QuarticIn01; break;
                case Function.QuarticOut: del = QuarticOut01; break;
                case Function.QuarticInOut: del = QuarticInOut01; break;
                case Function.QuinticIn: del = QuinticIn01; break;
                case Function.QuinticOut: del = QuinticOut01; break;
                case Function.QuinticInOut: del = QuinticInOut01; break;
                case Function.SineIn: del = SineIn01; break;
                case Function.SineOut: del = SineOut01; break;
                case Function.SineInOut: del = SineInOut01; break;
                case Function.ExponentialIn: del = ExponentialIn01; break;
                case Function.ExponentialOut: del = ExponentialOut01; break;
                case Function.ExponentialInOut: del = ExponentialInOut01; break;
                case Function.CircularIn: del = CircularIn01; break;
                case Function.CircularOut: del = CircularOut01; break;
                case Function.CircularInOut: del = CircularInOut01; break;
                case Function.BounceIn: del = BounceIn01; break;
                case Function.BounceOut: del = BounceOut01; break;
                case Function.BounceInOut: del = BounceInOut01; break;
                case Function.BackIn: del = BackIn01; break;
                case Function.BackOut: del = BackOut01; break;
                case Function.BackInOut: del = BackInOut01; break;
                case Function.ElasticIn: del = ElasticIn01; break;
                case Function.ElasticOut: del = ElasticOut01; break;
                case Function.ElasticInOut: del = ElasticInOut01; break;
                default: throw new ArgumentOutOfRangeException(nameof(function));
            }

            _Delegates01[i] = del;
            return del;
        }

        /************************************************************************************************************************/

        private static Delegate[] _Derivatives;

        /// <summary>Returns a cached delegate representing the derivative of the specified `function`.</summary>
        public static Delegate GetDelegateDerivative(this Function function)
        {
            var i = (int)function;
            Delegate del;

            if (_Derivatives == null)
            {
                _Derivatives = new Delegate[FunctionCount];
            }
            else
            {
                del = _Derivatives[i];
                if (del != null)
                    return del;
            }

            switch (function)
            {
                case Function.Linear: del = LinearDerivative; break;
                case Function.Spring: del = SpringDerivative; break;
                case Function.QuadraticIn: del = QuadraticInDerivative; break;
                case Function.QuadraticOut: del = QuadraticOutDerivative; break;
                case Function.QuadraticInOut: del = QuadraticInOutDerivative; break;
                case Function.CubicIn: del = CubicInDerivative; break;
                case Function.CubicOut: del = CubicOutDerivative; break;
                case Function.CubicInOut: del = CubicInOutDerivative; break;
                case Function.QuarticIn: del = QuarticInDerivative; break;
                case Function.QuarticOut: del = QuarticOutDerivative; break;
                case Function.QuarticInOut: del = QuarticInOutDerivative; break;
                case Function.QuinticIn: del = QuinticInDerivative; break;
                case Function.QuinticOut: del = QuinticOutDerivative; break;
                case Function.QuinticInOut: del = QuinticInOutDerivative; break;
                case Function.SineIn: del = SineInDerivative; break;
                case Function.SineOut: del = SineOutDerivative; break;
                case Function.SineInOut: del = SineInOutDerivative; break;
                case Function.ExponentialIn: del = ExponentialInDerivative; break;
                case Function.ExponentialOut: del = ExponentialOutDerivative; break;
                case Function.ExponentialInOut: del = ExponentialInOutDerivative; break;
                case Function.CircularIn: del = CircularInDerivative; break;
                case Function.CircularOut: del = CircularOutDerivative; break;
                case Function.CircularInOut: del = CircularInOutDerivative; break;
                case Function.BounceIn: del = BounceInDerivative; break;
                case Function.BounceOut: del = BounceOutDerivative; break;
                case Function.BounceInOut: del = BounceInOutDerivative; break;
                case Function.BackIn: del = BackInDerivative; break;
                case Function.BackOut: del = BackOutDerivative; break;
                case Function.BackInOut: del = BackInOutDerivative; break;
                case Function.ElasticIn: del = ElasticInDerivative; break;
                case Function.ElasticOut: del = ElasticOutDerivative; break;
                case Function.ElasticInOut: del = ElasticInOutDerivative; break;
                default: throw new ArgumentOutOfRangeException(nameof(function));
            }

            _Derivatives[i] = del;
            return del;
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Functions
        /************************************************************************************************************************/

        public static float Linear(float start, float end, float value)
        {
            return start + (end - start) * value;
        }

        public static float Spring(float start, float end, float value)
        {
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) *
                Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        /************************************************************************************************************************/

        public static float QuadraticIn(float start, float end, float value)
        {
            return start + (end - start) * value * value;
        }

        public static float QuadraticOut(float start, float end, float value)
        {
            return start - (end - start) * value * (value - 2);
        }

        public static float QuadraticInOut(float start, float end, float value)
        {
            value *= 2;
            end -= start;
            if (value < 1)
            {
                return start + end * 0.5f * value * value;
            }
            else
            {
                value--;
                return start - end * 0.5f * (value * (value - 2) - 1);
            }
        }

        /************************************************************************************************************************/

        public static float CubicIn(float start, float end, float value)
        {
            return start + (end - start) * value * value * value;
        }

        public static float CubicOut(float start, float end, float value)
        {
            value--;
            return start + (end - start) * (value * value * value + 1);
        }

        public static float CubicInOut(float start, float end, float value)
        {
            value *= 2;
            end -= start;
            if (value < 1)
            {
                return start + end * 0.5f * value * value * value;
            }
            else
            {
                value -= 2;
                return start + end * 0.5f * (value * value * value + 2);
            }
        }

        /************************************************************************************************************************/

        public static float QuarticIn(float start, float end, float value)
        {
            return start + (end - start) * value * value * value * value;
        }

        public static float QuarticOut(float start, float end, float value)
        {
            value--;
            return start - (end - start) * (value * value * value * value - 1);
        }

        public static float QuarticInOut(float start, float end, float value)
        {
            value *= 2;
            end -= start;
            if (value < 1)
            {
                return start + end * 0.5f * value * value * value * value;
            }
            else
            {
                value -= 2;
                return start - end * 0.5f * (value * value * value * value - 2);
            }
        }

        /************************************************************************************************************************/

        public static float QuinticIn(float start, float end, float value)
        {
            return start + (end - start) * value * value * value * value * value;
        }

        public static float QuinticOut(float start, float end, float value)
        {
            value--;
            return start + (end - start) * (value * value * value * value * value + 1);
        }

        public static float QuinticInOut(float start, float end, float value)
        {
            value *= 2;
            end -= start;
            if (value < 1)
            {
                return start + end * 0.5f * value * value * value * value * value;
            }
            else
            {
                value -= 2;
                return start + end * 0.5f * (value * value * value * value * value + 2);
            }
        }

        /************************************************************************************************************************/

        public static float SineIn(float start, float end, float value)
        {
            end -= start;
            return start - end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end;
        }

        public static float SineOut(float start, float end, float value)
        {
            return start + (end - start) * Mathf.Sin(value * (Mathf.PI * 0.5f));
        }

        public static float SineInOut(float start, float end, float value)
        {
            return start - (end - start) * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1);
        }

        /************************************************************************************************************************/

        public static float ExponentialIn(float start, float end, float value)
        {
            return start + (end - start) * Mathf.Pow(2, 10 * (value - 1));
        }

        public static float ExponentialOut(float start, float end, float value)
        {
            return start + (end - start) * (-Mathf.Pow(2, -10 * value) + 1);
        }

        public static float ExponentialInOut(float start, float end, float value)
        {
            value *= 2;
            end -= start;
            if (value < 1)
            {
                return start + end * 0.5f * Mathf.Pow(2, 10 * (value - 1));
            }
            else
            {
                value--;
                return start + end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2);
            }
        }

        /************************************************************************************************************************/

        public static float CircularIn(float start, float end, float value)
        {
            return start - (end - start) * (Mathf.Sqrt(1 - value * value) - 1);
        }

        public static float CircularOut(float start, float end, float value)
        {
            value--;
            return start + (end - start) * Mathf.Sqrt(1 - value * value);
        }

        public static float CircularInOut(float start, float end, float value)
        {
            value *= 2;
            end -= start;
            if (value < 1)
            {
                return start - end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1);
            }
            else
            {
                value -= 2;
                return start + end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1);
            }
        }

        /************************************************************************************************************************/

        public static float BounceIn(float start, float end, float value)
        {
            end -= start;
            return start + end - BounceOut(0, end, 1 - value);
        }

        public static float BounceOut(float start, float end, float value)
        {
            end -= start;
            if (value < (1 / 2.75f))
            {
                return start + end * (7.5625f * value * value);
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return start + end * (7.5625f * value * value + 0.75f);
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return start + end * (7.5625f * value * value + 0.9375f);
            }
            else
            {
                value -= (2.625f / 2.75f);
                return start + end * (7.5625f * value * value + 0.984375f);
            }
        }

        public static float BounceInOut(float start, float end, float value)
        {
            end -= start;
            if (value < 0.5f)
            {
                return start + BounceIn(0, end, value * 2) * 0.5f;
            }
            else
            {
                return start + BounceOut(0, end, value * 2 - 1) * 0.5f + end * 0.5f;
            }
        }

        /************************************************************************************************************************/

        private const float BackConstant = 1.70158f;

        public static float BackIn(float start, float end, float value)
        {
            return start + (end - start) * value * value * ((BackConstant + 1) * value - BackConstant);
        }

        public static float BackOut(float start, float end, float value)
        {
            value -= 1;
            return start + (end - start) * (value * value * ((BackConstant + 1) * value + BackConstant) + 1);
        }

        public static float BackInOut(float start, float end, float value)
        {
            value *= 2;
            if (value < 1)
            {
                return start + (end - start) * 0.5f * value * value * ((BackConstant + 1) * value - BackConstant);
            }
            else
            {
                value -= 2;
                return start + end * 0.5f * (value * value * ((BackConstant + 1) * value + BackConstant) + 2);
            }
        }

        /************************************************************************************************************************/

        public static float ElasticIn(float start, float end, float value)
        {
            end -= start;

            const float d = 1f;
            const float p = d * 0.3f;
            float s;
            float a = 0;

            if (Math.Abs(value) < float.Epsilon)
                return start;

            if (Math.Abs((value /= d) - 1) < float.Epsilon)
                return start + end;

            if (Math.Abs(a) < float.Epsilon || a < Math.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return start - (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p));
        }

        public static float ElasticOut(float start, float end, float value)
        {
            end -= start;

            const float d = 1f;
            const float p = d * 0.3f;
            float s;
            float a = 0;

            if (Math.Abs(value) < float.Epsilon)
                return start;

            if (Math.Abs((value /= d) - 1) < float.Epsilon)
                return start + end;

            if (Math.Abs(a) < float.Epsilon || a < Math.Abs(end))
            {
                a = end;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return start + end + a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p);
        }

        public static float ElasticInOut(float start, float end, float value)
        {
            end -= start;

            const float d = 1f;
            const float p = d * 0.3f;
            float s;
            float a = 0;

            if (Math.Abs(value) < float.Epsilon)
                return start;

            if (Math.Abs((value /= d * 0.5f) - 2) < float.Epsilon)
                return start + end;

            if (Math.Abs(a) < float.Epsilon || a < Math.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1)
                return start - 0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p));
            else
                return start + end + a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f;
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Functions 01
        // Copies of the original Functions using 0 and 1 as the start and end respectively.
        /************************************************************************************************************************/

        public static float Linear01(float value)
        {
            return value;
        }

        public static float Spring01(float value)
        {
            return (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) *
                Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
        }

        /************************************************************************************************************************/

        public static float QuadraticIn01(float value)
        {
            return value * value;
        }

        public static float QuadraticOut01(float value)
        {
            return -value * (value - 2);
        }

        public static float QuadraticInOut01(float value)
        {
            value *= 2;

            if (value < 1)
            {
                return 0.5f * value * value;
            }
            else
            {
                value--;
                return -0.5f * (value * (value - 2) - 1);
            }
        }

        /************************************************************************************************************************/

        public static float CubicIn01(float value)
        {
            return value * value * value;
        }

        public static float CubicOut01(float value)
        {
            value--;
            return 1 + value * value * value;
        }

        public static float CubicInOut01(float value)
        {
            value *= 2;

            if (value < 1)
            {
                return 0.5f * value * value * value;
            }
            else
            {
                value -= 2;
                return 0.5f * (value * value * value + 2);
            }
        }

        /************************************************************************************************************************/

        public static float QuarticIn01(float value)
        {
            return value * value * value * value;
        }

        public static float QuarticOut01(float value)
        {
            value--;
            return -(value * value * value * value - 1);
        }

        public static float QuarticInOut01(float value)
        {
            value *= 2;

            if (value < 1)
            {
                return 0.5f * value * value * value * value;
            }
            else
            {
                value -= 2;
                return -0.5f * (value * value * value * value - 2);
            }
        }

        /************************************************************************************************************************/

        public static float QuinticIn01(float value)
        {
            return value * value * value * value * value;
        }

        public static float QuinticOut01(float value)
        {
            value--;
            return (value * value * value * value * value + 1);
        }

        public static float QuinticInOut01(float value)
        {
            value *= 2;

            if (value < 1)
            {
                return 0.5f * value * value * value * value * value;
            }
            else
            {
                value -= 2;
                return 0.5f * (value * value * value * value * value + 2);
            }
        }

        /************************************************************************************************************************/

        public static float SineIn01(float value)
        {
            return -Mathf.Cos(value * (Mathf.PI * 0.5f)) + 1;
        }

        public static float SineOut01(float value)
        {
            return Mathf.Sin(value * (Mathf.PI * 0.5f));
        }

        public static float SineInOut01(float value)
        {
            return -0.5f * (Mathf.Cos(Mathf.PI * value) - 1);
        }

        /************************************************************************************************************************/

        public static float ExponentialIn01(float value)
        {
            return Mathf.Pow(2, 10 * (value - 1));
        }

        public static float ExponentialOut01(float value)
        {
            return (-Mathf.Pow(2, -10 * value) + 1);
        }

        public static float ExponentialInOut01(float value)
        {
            value *= 2;

            if (value < 1)
            {
                return 0.5f * Mathf.Pow(2, 10 * (value - 1));
            }
            else
            {
                value--;
                return 0.5f * (-Mathf.Pow(2, -10 * value) + 2);
            }
        }

        /************************************************************************************************************************/

        public static float CircularIn01(float value)
        {
            return -(Mathf.Sqrt(1 - value * value) - 1);
        }

        public static float CircularOut01(float value)
        {
            value--;
            return Mathf.Sqrt(1 - value * value);
        }

        public static float CircularInOut01(float value)
        {
            value *= 2;

            if (value < 1)
            {
                return -0.5f * (Mathf.Sqrt(1 - value * value) - 1);
            }
            else
            {
                value -= 2;
                return 0.5f * (Mathf.Sqrt(1 - value * value) + 1);
            }
        }

        /************************************************************************************************************************/

        public static float BounceIn01(float value)
        {
            const float d = 1f;
            return 1 - BounceOut01(d - value);
        }

        public static float BounceOut01(float value)
        {
            if (value < (1 / 2.75f))
            {
                return (7.5625f * value * value);
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return (7.5625f * value * value + 0.75f);
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return (7.5625f * value * value + 0.9375f);
            }
            else
            {
                value -= (2.625f / 2.75f);
                return (7.5625f * value * value + 0.984375f);
            }
        }

        public static float BounceInOut01(float value)
        {
            const float d = 1f;
            if (value < d * 0.5f)
            {
                return BounceIn01(value * 2) * 0.5f;
            }
            else
            {
                return BounceOut01(value * 2 - d) * 0.5f + 0.5f;
            }
        }

        /************************************************************************************************************************/

        public static float BackIn01(float value)
        {
            return value * value * ((BackConstant + 1) * value - BackConstant);
        }

        public static float BackOut01(float value)
        {
            value -= 1;
            return value * value * ((BackConstant + 1) * value + BackConstant) + 1;
        }

        public static float BackInOut01(float value)
        {
            value *= 2;
            if (value < 1)
            {
                return 0.5f * value * value * ((BackConstant + 1) * value - BackConstant);
            }
            else
            {
                value -= 2;
                return 0.5f * (value * value * ((BackConstant + 1) * value + BackConstant) + 2);
            }
        }

        /************************************************************************************************************************/

        public static float ElasticIn01(float value)
        {
            const float d = 1f;
            const float p = d * 0.3f;
            float s;
            float a = 0;

            if (Math.Abs(value) < float.Epsilon)
                return 0;

            if (Math.Abs((value /= d) - 1) < float.Epsilon)
                return 1;

            if (Math.Abs(a) < float.Epsilon || a < Math.Abs(1))
            {
                a = 1;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(1 / a);
            }

            return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p));
        }

        public static float ElasticOut01(float value)
        {
            const float d = 1f;
            const float p = d * 0.3f;
            float s;
            float a = 0;

            if (Math.Abs(value) < float.Epsilon)
                return 0;

            if (Math.Abs((value /= d) - 1) < float.Epsilon)
                return 1;

            if (Math.Abs(a) < float.Epsilon || a < Math.Abs(1))
            {
                a = 1;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(1 / a);
            }

            return 1 + a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p);
        }

        public static float ElasticInOut01(float value)
        {
            const float d = 1f;
            const float p = d * 0.3f;
            float s;
            float a = 0;

            if (Math.Abs(value) < float.Epsilon)
                return 0;

            if (Math.Abs((value /= d * 0.5f) - 2) < float.Epsilon)
                return 1;

            if (Math.Abs(a) < float.Epsilon || a < Math.Abs(1))
            {
                a = 1;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(1 / a);
            }

            if (value < 1)
                return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p));
            else
                return 1 + a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f;
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Derivatives
        /************************************************************************************************************************/

        public const float NaturalLogOf2 = 0.693147181f;

        /************************************************************************************************************************/

        public static float LinearDerivative(float start, float end, float value)
        {
            return end - start;
        }

        public static float SpringDerivative(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;

            // Damn... Thanks http://www.derivative-calculator.net/
            // And it's a little bit wrong
            return end * (6f * (1f - value) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - value, 1.2f) *
                Mathf.Sin(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + Mathf.Pow(1f - value, 2.2f) *
                (Mathf.PI * (2.5f * value * value * value + 0.2f) + 7.5f * Mathf.PI * value * value * value) *
                Mathf.Cos(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + 1f) -
                6f * end * (Mathf.Pow(1 - value, 2.2f) * Mathf.Sin(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + value
                / 5f);

        }

        /************************************************************************************************************************/

        public static float QuadraticInDerivative(float start, float end, float value)
        {
            return 2 * (end - start) * value;
        }

        public static float QuadraticOutDerivative(float start, float end, float value)
        {
            end -= start;
            return -end * value - end * (value - 2);
        }

        public static float QuadraticInOutDerivative(float start, float end, float value)
        {
            value *= 2;
            end -= start;

            if (value < 1)
            {
                return end * value;
            }
            else
            {
                value--;
                return end * (1 - value);
            }
        }

        /************************************************************************************************************************/

        public static float CubicInDerivative(float start, float end, float value)
        {
            return 3 * (end - start) * value * value;
        }

        public static float CubicOutDerivative(float start, float end, float value)
        {
            value--;
            return 3 * (end - start) * value * value;
        }

        public static float CubicInOutDerivative(float start, float end, float value)
        {
            value *= 2;
            end -= start;

            if (value < 1)
            {
                return (3f / 2f) * end * value * value;
            }
            else
            {
                value -= 2;

                return (3f / 2f) * end * value * value;
            }
        }

        /************************************************************************************************************************/

        public static float QuarticInDerivative(float start, float end, float value)
        {
            return 4 * (end - start) * value * value * value;
        }

        public static float QuarticOutDerivative(float start, float end, float value)
        {
            value--;
            return -4 * (end - start) * value * value * value;
        }

        public static float QuarticInOutDerivative(float start, float end, float value)
        {
            value *= 2;
            end -= start;

            if (value < 1)
            {
                return 2 * end * value * value * value;
            }
            else
            {
                value -= 2;

                return -2 * end * value * value * value;
            }
        }

        /************************************************************************************************************************/

        public static float QuinticInDerivative(float start, float end, float value)
        {
            return 5 * (end - start) * value * value * value * value;
        }

        public static float QuinticOutDerivative(float start, float end, float value)
        {
            value--;
            return 5 * (end - start) * value * value * value * value;
        }

        public static float QuinticInOutDerivative(float start, float end, float value)
        {
            value *= 2;
            end -= start;

            if (value < 1)
            {
                return (5f / 2f) * end * value * value * value * value;
            }
            else
            {
                value -= 2;

                return (5f / 2f) * end * value * value * value * value;
            }
        }

        /************************************************************************************************************************/

        public static float SineInDerivative(float start, float end, float value)
        {
            return (end - start) * 0.5f * Mathf.PI * Mathf.Sin(0.5f * Mathf.PI * value);
        }

        public static float SineOutDerivative(float start, float end, float value)
        {
            return (Mathf.PI * 0.5f) * (end - start) * Mathf.Cos(value * (Mathf.PI * 0.5f));
        }

        public static float SineInOutDerivative(float start, float end, float value)
        {
            return (end - start) * 0.5f * Mathf.PI * Mathf.Sin(Mathf.PI * value);
        }

        /************************************************************************************************************************/

        public static float ExponentialInDerivative(float start, float end, float value)
        {
            return 10 * NaturalLogOf2 * (end - start) * Mathf.Pow(2, 10 * (value - 1));
        }

        public static float ExponentialOutDerivative(float start, float end, float value)
        {
            return 5 * NaturalLogOf2 * (end - start) * Mathf.Pow(2, 1 - 10 * value);
        }

        public static float ExponentialInOutDerivative(float start, float end, float value)
        {
            value *= 2;
            end -= start;

            if (value < 1)
            {
                return 5f * NaturalLogOf2 * end * Mathf.Pow(2f, 10f * (value - 1));
            }
            else
            {
                value--;
                return (5f * NaturalLogOf2 * end) / (Mathf.Pow(2f, 10f * value));
            }
        }

        /************************************************************************************************************************/

        public static float CircularInDerivative(float start, float end, float value)
        {
            return (end - start) * value / Mathf.Sqrt(1f - value * value);
        }

        public static float CircularOutDerivative(float start, float end, float value)
        {
            value--;
            end -= start;
            return (-end * value) / Mathf.Sqrt(1 - value * value);
        }

        public static float CircularInOutDerivative(float start, float end, float value)
        {
            value *= 2;
            end -= start;

            if (value < 1)
            {
                return (end * value) / (2 * Mathf.Sqrt(1 - value * value));
            }
            else
            {
                value -= 2;
                return (-end * value) / (2 * Mathf.Sqrt(1 - value * value));
            }
        }

        /************************************************************************************************************************/

        public static float BounceInDerivative(float start, float end, float value)
        {
            const float d = 1f;
            return BounceOutDerivative(0, end - start, d - value);
        }

        public static float BounceOutDerivative(float start, float end, float value)
        {
            end -= start;

            if (value < (1 / 2.75f))
            {
                return 2f * end * 7.5625f * value;
            }
            else if (value < (2 / 2.75f))
            {
                value -= (1.5f / 2.75f);
                return 2f * end * 7.5625f * value;
            }
            else if (value < (2.5 / 2.75))
            {
                value -= (2.25f / 2.75f);
                return 2f * end * 7.5625f * value;
            }
            else
            {
                value -= (2.625f / 2.75f);
                return 2f * end * 7.5625f * value;
            }
        }

        public static float BounceInOutDerivative(float start, float end, float value)
        {
            end -= start;
            const float d = 1f;

            if (value < d * 0.5f)
                return BounceInDerivative(0, end, value * 2) * 0.5f;
            else
                return BounceOutDerivative(0, end, value * 2 - d) * 0.5f;
        }

        /************************************************************************************************************************/

        public static float BackInDerivative(float start, float end, float value)
        {
            return 3 * (BackConstant + 1) * (end - start) * value * value - 2 * BackConstant * (end - start) * value;
        }

        public static float BackOutDerivative(float start, float end, float value)
        {
            value -= 1;

            return (end - start) * ((BackConstant + 1f) * value * value + 2f * value * ((BackConstant + 1f) * value + BackConstant));
        }

        public static float BackInOutDerivative(float start, float end, float value)
        {
            end -= start;
            value *= 2;

            if (value < 1)
            {
                return 0.5f * end * (BackConstant + 1) * value * value + end * value * ((BackConstant + 1) * value - BackConstant);
            }
            else
            {
                value -= 2;
                return 0.5f * end * ((BackConstant + 1) * value * value + 2f * value * ((BackConstant + 1) * value + BackConstant));
            }
        }

        /************************************************************************************************************************/

        public static float ElasticInDerivative(float start, float end, float value)
        {
            return ElasticOutDerivative(start, end, 1 - value);
        }

        public static float ElasticOutDerivative(float start, float end, float value)
        {
            end -= start;

            const float d = 1;
            const float p = d * 0.3f;
            float s;
            float a = 0;

            if (Math.Abs(a) < float.Epsilon || a < Math.Abs(end))
            {
                a = end;
                s = p * 0.25f;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.PI * d * Mathf.Pow(2f, 1f - 10f * value) *
                Mathf.Cos((2f * Mathf.PI * (d * value - s)) / p)) / p - 5f * NaturalLogOf2 * a *
                Mathf.Pow(2f, 1f - 10f * value) * Mathf.Sin((2f * Mathf.PI * (d * value - s)) / p);
        }

        public static float ElasticInOutDerivative(float start, float end, float value)
        {
            end -= start;

            const float d = 1f;
            const float p = d * 0.3f;
            float s;
            float a = 0;

            if (Math.Abs(a) < float.Epsilon || a < Math.Abs(end))
            {
                a = end;
                s = p / 4;
            }
            else
            {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1)
            {
                value -= 1;

                return -5f * NaturalLogOf2 * a * Mathf.Pow(2f, 10f * value) * Mathf.Sin(2 * Mathf.PI * (d * value - 2f) / p) -
                    a * Mathf.PI * d * Mathf.Pow(2f, 10f * value) * Mathf.Cos(2 * Mathf.PI * (d * value - s) / p) / p;
            }
            else
            {
                value -= 1;

                return a * Mathf.PI * d * Mathf.Cos(2f * Mathf.PI * (d * value - s) / p) / (p * Mathf.Pow(2f, 10f * value)) -
                    5f * NaturalLogOf2 * a * Mathf.Sin(2f * Mathf.PI * (d * value - s) / p) / (Mathf.Pow(2f, 10f * value));
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}
