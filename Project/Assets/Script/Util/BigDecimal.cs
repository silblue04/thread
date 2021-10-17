﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// https://github.com/SuprDewd/SharpBag/blob/master/SharpBag/Math/BigDecimal.cs
namespace System.Numerics
{
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Numerics;

    /// <summary>
    /// An arbitrary big decimal.
    /// </summary>
    public struct BigDecimal : IComparable<BigDecimal>, IEquatable<BigDecimal>
    {
        #region Properties

        private const int DefaultPrecision = 32;

        private const int ExtraPrecision = 20;

        private const int Radix = 10;

        private const int RadixPow2 = 100;

        private int _Precision;

        /// <summary>
        /// The maximum amount of digits after the decimal point.
        /// </summary>
        public int Precision
        {
            get { return _Precision; }
            private set
            {
                bool fix = value < _Precision;
                _Precision = value;
                if (fix) this.FixPrecision();
            }
        }

        private BigInteger _Mantissa;

        private BigInteger Mantissa { get { return _Mantissa; } set { _Mantissa = value; } }

        private int _Exponent;

        private int Exponent { get { return _Exponent; } set { _Exponent = value; } }

        private bool _Normalized;

        private bool Normalized { get { return _Normalized; } set { _Normalized = value; } }

        private bool _UsingDefaultPrecision;

        private bool UsingDefaultPrecision { get { return _UsingDefaultPrecision; } set { _UsingDefaultPrecision = value; } }

        #endregion Properties

        #region Static Instances

        /// <summary>
        /// A positive one.
        /// </summary>
        public static readonly BigDecimal One = new BigDecimal(1);

        /// <summary>
        /// A negative one.
        /// </summary>
        public static readonly BigDecimal MinusOne = new BigDecimal(-1);

        /// <summary>
        /// A zero.
        /// </summary>
        public static readonly BigDecimal Zero = new BigDecimal(0);

        #endregion Static Instances

        #region Constructors / Factories

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        public BigDecimal(int value) : this(value, DefaultPrecision, true) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        /// <param name="precision">The maximum amount of digits after the decimal point.</param>
        public BigDecimal(int value, int precision) : this(value, precision, false) { }

        private BigDecimal(int value, int precision, bool defaultPrecision) : this(value, 0, precision, false, defaultPrecision) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        public BigDecimal(long value) : this(value, DefaultPrecision, true) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        /// <param name="precision">The maximum amount of digits after the decimal point.</param>
        public BigDecimal(long value, int precision) : this(value, precision, false) { }

        private BigDecimal(long value, int precision, bool defaultPrecision) : this(value, 0, precision, false, defaultPrecision) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        public BigDecimal(BigInteger value) : this(value, DefaultPrecision, true) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        /// <param name="precision">The maximum amount of digits after the decimal point.</param>
        public BigDecimal(BigInteger value, int precision) : this(value, precision, false) { }

        private BigDecimal(BigInteger value, int precision, bool defaultPrecision) : this(value, 0, precision, false, defaultPrecision) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        public BigDecimal(float value) : this(value, DefaultPrecision, false, true) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        /// <param name="precision">The maximum amount of digits after the decimal point.</param>
        public BigDecimal(float value, int precision) : this(value, precision, false, false) { }

        private BigDecimal(float value, int precision, bool normalized, bool defaultPrecision)
        {
            Contract.Requires(precision >= 0);
            string[] srep = value.ToString("R").Split('E');
            BigDecimal parsed = BigDecimal.Parse(srep[0]);

            _ToStringCache = null;
            _Mantissa = parsed.Mantissa;
            _Exponent = srep.Length == 2 ? (parsed.Exponent + Convert.ToInt32(srep[1])) : parsed.Exponent;
            _Precision = precision;
            _Normalized = normalized;
            _UsingDefaultPrecision = defaultPrecision;

            this.FixPrecision();
            this.Normalize();
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        public BigDecimal(double value) : this(value, DefaultPrecision, false, true) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        /// <param name="precision">The maximum amount of digits after the decimal point.</param>
        public BigDecimal(double value, int precision) : this(value, precision, false, false) { }

        private BigDecimal(double value, int precision, bool normalized, bool defaultPrecision)
        {
            Contract.Requires(precision >= 0);
            string[] srep = value.ToString("R").Split('E');
            BigDecimal parsed = BigDecimal.Parse(srep[0]);

            _ToStringCache = null;
            _Mantissa = parsed.Mantissa;
            _Exponent = srep.Length == 2 ? (parsed.Exponent + Convert.ToInt32(srep[1])) : parsed.Exponent;
            _Precision = precision;
            _Normalized = normalized;
            _UsingDefaultPrecision = defaultPrecision;

            this.FixPrecision();
            this.Normalize();
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        public BigDecimal(decimal value) : this(value, DefaultPrecision, false, true) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        /// <param name="precision">The maximum amount of digits after the decimal point.</param>
        public BigDecimal(decimal value, int precision) : this(value, precision, false, false) { }

        private BigDecimal(decimal value, int precision, bool normalized, bool defaultPrecision)
        {
            Contract.Requires(precision >= 0);
            string[] srep = value.ToString().Split('E');
            BigDecimal parsed = BigDecimal.Parse(srep[0]);

            _ToStringCache = null;
            _Mantissa = parsed.Mantissa;
            _Exponent = srep.Length == 2 ? (parsed.Exponent + Convert.ToInt32(srep[1])) : parsed.Exponent;
            _Precision = precision;
            _Normalized = normalized;
            _UsingDefaultPrecision = defaultPrecision;

            this.FixPrecision();
            this.Normalize();
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        public BigDecimal(BigDecimal value) : this(value.Mantissa, value.Exponent, value.Precision, value.Normalized, value.UsingDefaultPrecision) { }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="value">The value of the decimal.</param>
        /// <param name="precision">The maximum amount of digits after the decimal point.</param>
        public BigDecimal(BigDecimal value, int precision) : this(value.Mantissa, value.Exponent, precision, value.Normalized, false) { }

        private BigDecimal(BigInteger value, int exponent, int precision, bool normalized, bool defaultPrecision)
        {
            Contract.Requires(precision >= 0);
            _ToStringCache = null;
            _Mantissa = value;
            _Exponent = exponent;
            _Precision = precision;
            _Normalized = normalized;
            _UsingDefaultPrecision = defaultPrecision;

            this.FixPrecision();
            this.Normalize();
        }

        /// <summary>
        /// Parse the specified string.
        /// </summary>
        /// <param name="value">The specified string.</param>
        /// <returns>The BigDecimal represented by the string.</returns>
        public static BigDecimal Parse(string value)
        {
            value = value.Trim();
            int exp = 0;
            BigInteger mantissa = 0;
            bool foundComma = false;
            int i = 0;
            bool neg = false;
            if (value[0] == CultureInfo.CurrentCulture.NumberFormat.NegativeSign[0])
            {
                neg = true;
                i++;
            }

            for (; i < value.Length; i++)
            {
                if (value[i] == '.' || value[i] == ',')
                {
                    if (foundComma) throw new FormatException();
                    foundComma = true;
                    continue;
                }

                if (!Char.IsDigit(value[i])) throw new FormatException();
                if (foundComma) exp--;
                mantissa *= 10;
                mantissa += value[i] - '0';
            }

            bool defaultP = false;
            int precision = -exp;
            if (precision < DefaultPrecision)
            {
                precision = DefaultPrecision;
                defaultP = true;
            }

            return new BigDecimal(neg ? -mantissa : mantissa, exp, precision, false, defaultP);
        }

        /// <summary>
        /// Parse the specified string.
        /// </summary>
        /// <param name="value">The specified string.</param>
        /// <param name="precision">The maximum amount of digits after the decimal point.</param>
        /// <returns>The BigDecimal represented by the string.</returns>
        public static BigDecimal Parse(string value, int precision)
        {
            Contract.Requires(precision >= 0);
            value = value.Trim();
            int exp = 0;
            BigInteger mantissa = 0;
            bool foundComma = false;
            int i = 0;
            bool neg = false;
            if (value[0] == CultureInfo.CurrentCulture.NumberFormat.NegativeSign[0])
            {
                neg = true;
                i++;
            }

            for (; i < value.Length; i++)
            {
                if (value[i] == '.' || value[i] == ',')
                {
                    if (foundComma) throw new FormatException();
                    foundComma = true;
                    continue;
                }

                if (!Char.IsDigit(value[i])) throw new FormatException();
                if (foundComma) exp--;
                mantissa *= 10;
                mantissa += value[i] - '0';
            }

            return new BigDecimal(neg ? -mantissa : mantissa, exp, precision, false, false);
        }

        #endregion Constructors / Factories

        #region Operators

        /// <summary>
        /// The + operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>Left + Right</returns>
        public static BigDecimal operator +(BigDecimal left, BigDecimal right) { return left.Add(right); }

        /// <summary>
        /// The - operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>Left - Right</returns>
        public static BigDecimal operator -(BigDecimal left, BigDecimal right) { return left.Subtract(right); }

        /// <summary>
        /// The * operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>Left * Right</returns>
        public static BigDecimal operator *(BigDecimal left, BigDecimal right) { return left.Multiply(right); }

        /// <summary>
        /// The / operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>Left / Right</returns>
        public static BigDecimal operator /(BigDecimal left, BigDecimal right) { return left.Divide(right); }

        /// <summary>
        /// The - operator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>-Left</returns>
        public static BigDecimal operator -(BigDecimal value) { return value.Negate(); }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="left">The left number.</param>
        /// <param name="right">The right number.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static BigDecimal operator %(BigDecimal left, BigDecimal right) { return BigDecimal.Remainder(left, right); }

        private BigDecimal Add(BigDecimal right, bool normalize)
        {
            BigDecimal.Align(ref this, ref right);
            BigDecimal result = new BigDecimal(this.Mantissa + right.Mantissa, this.Exponent, BigDecimal.PrecisionFor(ref this, ref right), false, this.UsingDefaultPrecision != right.UsingDefaultPrecision || this.UsingDefaultPrecision);

            if (normalize)
            {
                this.Normalize();
                right.Normalize();
            }

            return result;
        }

        private BigDecimal Add(BigDecimal right)
        {
            return this.Add(right, true);
        }

        private BigDecimal Subtract(BigDecimal right, bool normalize)
        {
            BigDecimal.Align(ref this, ref right);
            BigDecimal result = new BigDecimal(this.Mantissa - right.Mantissa, this.Exponent, BigDecimal.PrecisionFor(ref this, ref right), false, this.UsingDefaultPrecision != right.UsingDefaultPrecision || this.UsingDefaultPrecision);

            if (normalize)
            {
                this.Normalize();
                right.Normalize();
            }

            return result;
        }

        private BigDecimal Subtract(BigDecimal right)
        {
            return this.Subtract(right, true);
        }

        private BigDecimal Negate()
        {
            return new BigDecimal(-this.Mantissa, this.Exponent, this.Precision, this.Normalized, this.UsingDefaultPrecision);
        }

        private BigDecimal Multiply(BigDecimal right)
        {
            int z = this.Exponent + right.Exponent;
            BigInteger m = this.Mantissa * right.Mantissa;
            return new BigDecimal(m, z, BigDecimal.PrecisionFor(ref this, ref right), false, this.UsingDefaultPrecision != right.UsingDefaultPrecision || this.UsingDefaultPrecision);
        }

        private BigDecimal Divide(BigDecimal right)
        {
            int precision = BigDecimal.PrecisionFor(ref this, ref right) + BigDecimal.ExtraPrecision + 2,
                exponent = this.Exponent - right.Exponent,
                iterations = 0;

            bool leftPos = this.Mantissa >= 0,
                 rightPos = right.Mantissa >= 0,
                 pos = !(leftPos ^ rightPos);

            BigInteger remainder,
                       mantissa = BigInteger.DivRem(leftPos ? this.Mantissa : -this.Mantissa, rightPos ? right.Mantissa : -right.Mantissa, out remainder);

            while (remainder > 0 && iterations < precision)
            {
                exponent--;
                mantissa *= 10;
                remainder *= 10;
                mantissa += BigInteger.DivRem(remainder, rightPos ? right.Mantissa : -right.Mantissa, out remainder);
                if (mantissa != 0) iterations++;
            }

            return new BigDecimal(pos ? mantissa : -mantissa, exponent, precision - BigDecimal.ExtraPrecision - 2, false, this.UsingDefaultPrecision != right.UsingDefaultPrecision || this.UsingDefaultPrecision);
        }

        /// <summary>
        /// Computes the absolute value of the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The absolute value.</returns>
        public static BigDecimal Abs(BigDecimal value)
        {
            return new BigDecimal(BigInteger.Abs(value.Mantissa), value.Exponent, value.Precision, value.Normalized, value.UsingDefaultPrecision);
        }

        /// <summary>
        /// Computes the left number divided by the right number and the remainder of that divison.
        /// </summary>
        /// <param name="left">The left number.</param>
        /// <param name="right">The right number.</param>
        /// <param name="remainder">The remainder.</param>
        /// <returns>The result of the division.</returns>
        public static BigDecimal DivRem(BigDecimal left, BigDecimal right, out BigDecimal remainder)
        {
            BigDecimal.Align(ref left, ref right);
            int precision = BigDecimal.PrecisionFor(ref left, ref right);

            bool leftPos = left.Mantissa >= 0,
                 rightPos = right.Mantissa >= 0,
                 pos = !(leftPos ^ rightPos);

            BigInteger curRemainder,
                       mantissa = BigInteger.DivRem(leftPos ? left.Mantissa : -left.Mantissa, rightPos ? right.Mantissa : -right.Mantissa, out curRemainder);

            remainder = new BigDecimal(leftPos ? curRemainder : -curRemainder, left.Exponent, precision, false, false);
            return new BigDecimal(pos ? mantissa : -mantissa, 0, precision, false, false);
        }

        /// <summary>
        /// Computes the remainder of the division of the two numbers.
        /// </summary>
        /// <param name="left">The left number.</param>
        /// <param name="right">The right number.</param>
        /// <returns>The remainder.</returns>
        public static BigDecimal Remainder(BigDecimal left, BigDecimal right)
        {
            BigDecimal rem;
            BigDecimal.DivRem(left, right, out rem);
            return rem;
        }

        /// <summary>
        /// Raise the BigDecimal to the specified power.
        /// </summary>
        /// <param name="value">The BigDecimal.</param>
        /// <param name="power">The power.</param>
        /// <returns>The BigDecimal raised to the specified power.</returns>
        public static BigDecimal Pow(BigDecimal value, int power)
        {
            if (power < 0) return BigDecimal.Reciprocal(BigDecimal.Pow(value, -power));
            if (power == 0) return new BigDecimal(1, 0, value.Precision, true, value.UsingDefaultPrecision);
            if (power == 1) return new BigDecimal(value);
            if (power == 2) return value * value;
            if (power == 3) return value * value * value;

            if (power % 2 == 0)
            {
                BigDecimal temp = BigDecimal.Pow(value, power / 2);
                return temp * temp;
            }
            else
            {
                BigDecimal temp = BigDecimal.Pow(value, (power - 1) / 2);
                return temp * temp * value;
            }
        }

        /// <summary>
        /// Raise the BigDecimal to the specified power.
        /// </summary>
        /// <param name="value">The BigDecimal.</param>
        /// <param name="power">The power.</param>
        /// <returns>The BigDecimal raised to the specified power.</returns>
        public static BigDecimal Pow(BigDecimal value, BigDecimal power)
        {
            if (power < 0) return BigDecimal.Reciprocal(BigDecimal.Pow(value, -power));
            if (power == 0) return new BigDecimal(1, 0, value.Precision, true, value.UsingDefaultPrecision);
            if (power == 1) return new BigDecimal(value);
            return BigDecimal.Exp(BigDecimal.Log(value) * power);
        }

        /// <summary>
        /// The base-10 logarithm of the BigDecimal.
        /// </summary>
        /// <param name="value">The BigDecimal.</param>
        /// <returns>The base-10 logarithm of the BigDecimal.</returns>
        public static BigDecimal Log10(BigDecimal value)
        {
            if (value < 1) throw new ArgumentOutOfRangeException("value");
            int a = (int)BigInteger.Log10(value.Mantissa) + value.Exponent;
            BigDecimal m = value;
            BigInteger mantissa = a;
            int digits = 0,
                precision = value.Precision + BigDecimal.ExtraPrecision;

            while (digits < precision)
            {
                m = BigDecimal.Pow(m / BigInteger.Pow(10, a), 10);
                a = (int)BigInteger.Log10(m.Mantissa) + m.Exponent;
                mantissa = mantissa * 10 + a;
                digits++;
            }

            return new BigDecimal(mantissa, -digits, value.Precision, false, value.UsingDefaultPrecision);
        }

        /// <summary>
        /// Calculates the natural logarithm of the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The natural logarithm.</returns>
        public static BigDecimal Log(BigDecimal value)
        {
            return BigDecimal.Log10(value) / Constants.Log10EBig(value.Precision);
        }

        /// <summary>
        /// The logarithm of the BigDecimal in the specified base.
        /// </summary>
        /// <param name="value">The BigDecimal.</param>
        /// <param name="logBase">The specified base.</param>
        /// <returns>The logarithm of the BigDecimal in the specified base.</returns>
        public static BigDecimal Log(BigDecimal value, BigDecimal logBase)
        {
            int precision = BigDecimal.PrecisionFor(ref value, ref logBase);
            return BigDecimal.Log10(value.WithPrecision(precision)) / BigDecimal.Log10(logBase.WithPrecision(precision));
        }

        /// <summary>
        /// The square root of the BigDecimal.
        /// </summary>
        /// <param name="value">The BigDecimal.</param>
        /// <returns>The square root.</returns>
        public static BigDecimal Sqrt(BigDecimal value)
        {
            Contract.Requires(value >= 0);
            if (value.Mantissa == 0) return new BigDecimal(0, 0, value.Precision, true, value.UsingDefaultPrecision);
            BigDecimal sqrt = new BigDecimal(1, value.Precision + 1), last, two = new BigDecimal(2, value.Precision + 1);

            do
            {
                last = sqrt;
                sqrt = sqrt - ((sqrt * sqrt) - value) / (two * sqrt);
            }
            while (sqrt != last);

            return sqrt.WithPrecision(value.Precision);
        }

        /// <summary>
        /// The reciprocal of the BigDecimal.
        /// </summary>
        /// <param name="value">The BigDecimal.</param>
        /// <returns>The reciprocal of the BigDecimal.</returns>
        public static BigDecimal Reciprocal(BigDecimal value)
        {
            return BigDecimal.One / value;
        }

        /// <summary>
        /// Calculates the exponential function of the BigDecimal.
        /// </summary>
        /// <param name="value">The BigDecimal.</param>
        /// <returns>e ^ x</returns>
        public static BigDecimal Exp(BigDecimal value)
        {
            BigDecimal result = value.WithPrecision(value.Precision + 4) + BigDecimal.One,
                       lastResult, factorial = BigDecimal.One;

            int n = 2;

            do
            {
                lastResult = result;
                factorial = factorial * n;
                result = result + BigDecimal.Pow(value, n) / factorial;
                n++;
            } while (result != lastResult);

            result = result.WithPrecision(value.Precision);
            result._UsingDefaultPrecision = value._UsingDefaultPrecision;
            return BigDecimal.Round(result, value.Precision + 1);
        }

        /// <summary>
        /// Computes the sine of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The sine of the specified value.</returns>
        public static BigDecimal Sin(BigDecimal value)
        {
            BigDecimal result = value,
                       result2 = result * result,
                       lastResult,
                       it = BigDecimal.One.WithPrecision(value.Precision),
                       fact = BigDecimal.One.WithPrecision(value.Precision),
                       pow = value;

            bool alt = false;

            do
            {
                lastResult = result;
                pow *= result2;
                fact *= (it + 1) * (it + 2);

                if (alt) result += pow / fact;
                else result -= pow / fact;
                alt = !alt;
                it += 2;
            }
            while (result != lastResult);

            if (BigDecimal.Abs(result) > 1) throw new ArgumentOutOfRangeException("value must be between -2pi and 2pi");
            return result;
        }

        /// <summary>
        /// Computes the cosine of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The cosine of the specified value.</returns>
        public static BigDecimal Cos(BigDecimal value)
        {
            BigDecimal result = BigDecimal.One.WithPrecision(value.Precision),
                       result2 = value * value,
                       lastResult,
                       it = BigDecimal.Zero.WithPrecision(value.Precision),
                       fact = BigDecimal.One.WithPrecision(value.Precision),
                       pow = result;

            bool alt = false;

            do
            {
                lastResult = result;
                pow *= result2;
                fact *= (it + 1) * (it + 2);

                if (alt) result += pow / fact;
                else result -= pow / fact;
                alt = !alt;
                it += 2;
            }
            while (result != lastResult);

            if (BigDecimal.Abs(result) > 1) throw new ArgumentOutOfRangeException("value must be between -2pi and 2pi");
            return result;
        }

        /// <summary>
        /// Computes the tangent of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The tangent of the specified value.</returns>
        public static BigDecimal Tan(BigDecimal value)
        {
            try
            {
                return BigDecimal.Sin(value) / BigDecimal.Cos(value);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        /// <summary>
        /// Rounds the BigDecimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="digits">The amount of digits to keep.</param>
        /// <returns>The rounded value.</returns>
        public static BigDecimal Round(BigDecimal value, int digits = 0)
        {
            Contract.Requires(digits >= 0);
            if (digits >= value.Precision + BigDecimal.ExtraPrecision) return value;
            int prec = value.Precision;
            value = new BigDecimal(value.Mantissa, value.Exponent, digits + 1, false, false).WithoutExtraPrecision();

            if (-value.Exponent >= value.Precision)
            {
                int last = (int)(value.Mantissa % 10);
                if (last >= 5) value.Mantissa += 10 - last;
                else if (last >= 1) value.Mantissa -= last;
            }

            return new BigDecimal(value.Mantissa, value.Exponent, prec, false, value._UsingDefaultPrecision);
        }

        /*
        * myung_ari
        * Round 변형
        */
        public static BigDecimal Floor(BigDecimal value, int digits = 0)
        {
            Contract.Requires(digits >= 0);
            if (digits >= value.Precision + BigDecimal.ExtraPrecision) return value;
            int prec = value.Precision;
            value = new BigDecimal(value.Mantissa, value.Exponent, digits + 1, false, false).WithoutExtraPrecision();

            if (-value.Exponent >= value.Precision)
            {
                int last = (int)(value.Mantissa % 10);
                value.Mantissa -= last;
            }

            return new BigDecimal(value.Mantissa, value.Exponent, prec, false, value._UsingDefaultPrecision);
        }

#endregion Operators

        #region Ordering

        /// <summary>
        /// The > operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>True if Left > Right</returns>
        public static bool operator >(BigDecimal left, BigDecimal right) { return left.CompareTo(right) > 0; }

        /// <summary>
        /// The >= operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>True if Left >= Right</returns>
        public static bool operator >=(BigDecimal left, BigDecimal right) { return left.CompareTo(right) >= 0; }

        /// <summary>
        /// The &lt; operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>True if Left %lt; Right</returns>
        public static bool operator <(BigDecimal left, BigDecimal right) { return left.CompareTo(right) < 0; }

        /// <summary>
        /// The &lt;= operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>True if Left %lt;= Right</returns>
        public static bool operator <=(BigDecimal left, BigDecimal right) { return left.CompareTo(right) <= 0; }

        /// <summary>
        /// The == operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>True if Left == Right</returns>
        public static bool operator ==(BigDecimal left, BigDecimal right) { return left.Equals(right); }

        /// <summary>
        /// The != operator.
        /// </summary>
        /// <param name="left">The left BigDecimal.</param>
        /// <param name="right">The right BigDecimal.</param>
        /// <returns>True if Left != Right</returns>
        public static bool operator !=(BigDecimal left, BigDecimal right) { return !left.Equals(right); }

        /// <summary>
        /// IEquatable.Equals()
        /// </summary>
        /// <param name="other">Another BigDecimal.</param>
        /// <returns>Wheter this is equal to the other BigDecimal.</returns>
        public bool Equals(BigDecimal other)
        {
            BigDecimal left = this.WithoutExtraPrecision(),
                       right = other.WithoutExtraPrecision();

            BigDecimal.Align(ref left, ref right);
            return left.Mantissa.Equals(right.Mantissa);
        }

        /// <summary>
        /// Object.Equals()
        /// </summary>
        /// <param name="obj">Another BigDecimal.</param>
        /// <returns>Wheter this is equal to the other BigDecimal.</returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(BigDecimal) && this.Equals((BigDecimal)obj);
        }

        /// <summary>
        /// IComparable.CompareTo()
        /// </summary>
        /// <param name="other">Another BigDecimal.</param>
        /// <returns>The order of the BigDecimals.</returns>
        public int CompareTo(BigDecimal other)
        {
            BigDecimal left = this.WithoutExtraPrecision(),
                       right = other.WithoutExtraPrecision();
            BigDecimal.Align(ref left, ref right);
            return left.Mantissa.CompareTo(right.Mantissa);
        }

        #endregion Ordering

        #region Casting

        /// <summary>
        /// A casting operator.
        /// </summary>
        /// <param name="n">A value.</param>
        /// <returns>The result.</returns>
        public static implicit operator BigDecimal(int n) { return new BigDecimal(n); }

        /// <summary>
        /// A casting operator.
        /// </summary>
        /// <param name="n">A value.</param>
        /// <returns>The result.</returns>
        public static implicit operator BigDecimal(long n) { return new BigDecimal(n); }

        /// <summary>
        /// A casting operator.
        /// </summary>
        /// <param name="n">A value.</param>
        /// <returns>The result.</returns>
        public static implicit operator BigDecimal(BigInteger n) { return new BigDecimal(n); }

        /// <summary>
        /// A casting operator.
        /// </summary>
        /// <param name="n">A value.</param>
        /// <returns>The result.</returns>
        public static implicit operator BigDecimal(float n) { return new BigDecimal(n); }

        /// <summary>
        /// A casting operator.
        /// </summary>
        /// <param name="n">A value.</param>
        /// <returns>The result.</returns>
        public static implicit operator BigDecimal(double n) { return new BigDecimal(n); }

        /// <summary>
        /// A casting operator.
        /// </summary>
        /// <param name="n">A value.</param>
        /// <returns>The result.</returns>
        public static implicit operator BigDecimal(decimal n) { return new BigDecimal(n); }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpBag.Math.BigDecimal"/> to <see cref="System.Numerics.BigInteger"/>.
        /// </summary>
        /// <param name="n">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator BigInteger(BigDecimal n)
        {
            n = BigDecimal.Round(n, 0);
            return n.Mantissa * BigInteger.Pow(BigDecimal.Radix, n.Exponent);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpBag.Math.BigDecimal"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="n">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator int(BigDecimal n) { return (int)(BigInteger)n; }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpBag.Math.BigDecimal"/> to <see cref="System.Int64"/>.
        /// </summary>
        /// <param name="n">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator long(BigDecimal n) { return (long)(BigInteger)n; }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpBag.Math.BigDecimal"/> to <see cref="System.Decimal"/>.
        /// </summary>
        /// <param name="n">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator decimal(BigDecimal n)
        {
            string s = n.ToString();
            decimal res = 0, mod = 1;
            bool found = false;

            for (int i = 0; i < s.Length; i++)
            {
                if (found) res += (s[i] - '0') * (mod /= 10);
                else if (s[i] == ',' || s[i] == '.') found = true;
                else res = res * 10 + (s[i] - '0');
            }

            return res;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpBag.Math.BigDecimal"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="n">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator double(BigDecimal n)
        {
            string s = n.ToString();
            double res = 0, mod = 1;
            bool found = false;

            for (int i = 0; i < s.Length; i++)
            {
                if (found) res += (s[i] - '0') * (mod /= 10);
                else if (s[i] == ',' || s[i] == '.') found = true;
                else res = res * 10 + (s[i] - '0');
            }

            return res;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SharpBag.Math.BigDecimal"/> to <see cref="System.Single"/>.
        /// </summary>
        /// <param name="n">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator float(BigDecimal n)
        {
            string s = n.ToString();
            float res = 0, mod = 1;
            bool found = false;

            for (int i = 0; i < s.Length; i++)
            {
                if (found) res += (s[i] - '0') * (mod /= 10);
                else if (s[i] == ',' || s[i] == '.') found = true;
                else res = res * 10 + (s[i] - '0');
            }

            return res;
        }

        #endregion Casting

        #region Other

        private void FixPrecision()
        {
            if (this.Exponent < 0)
            {
                int n = -this.Exponent - this.Precision - BigDecimal.ExtraPrecision;
                if (n > 0)
                {
                    this.Mantissa /= BigInteger.Pow(10, n);
                    this.Exponent += n;
                }
            }
        }

        private BigDecimal WithoutExtraPrecision()
        {
            BigInteger mantissa = this.Mantissa;
            int exponent = this.Exponent;

            if (exponent < 0)
            {
                int n = -exponent - this.Precision;
                if (n > 0)
                {
                    mantissa /= BigInteger.Pow(10, n);
                    exponent += n;
                }
            }

            return new BigDecimal(mantissa, exponent, this.Precision, false, this.UsingDefaultPrecision);
        }

        private void Expand()
        {
            if (this.Exponent > 0)
            {
                this.Mantissa *= BigInteger.Pow(10, this.Exponent);
                this.Exponent = 0;
            }
        }

        private void Normalize()
        {
            if (!this.Normalized)
            {
                if (this.Mantissa != 0)
                {
                    while (this.Mantissa % RadixPow2 == 0)
                    {
                        this.Exponent += 2;
                        this.Mantissa /= RadixPow2;
                    }

                    if (this.Mantissa % Radix == 0)
                    {
                        this.Exponent++;
                        this.Mantissa /= Radix;
                    }
                }
                else
                {
                    this.Exponent = 0;
                }

                this.Normalized = true;
            }
        }

        private void AlignWith(BigDecimal other)
        {
            if (this.Exponent > other.Exponent)
            {
                this.Mantissa *= BigInteger.Pow(Radix, this.Exponent - other.Exponent);
                this.Exponent = other.Exponent;
            }
        }

        private static void Align(ref BigDecimal a, ref BigDecimal b)
        {
            if (a.Exponent > b.Exponent) a.AlignWith(b);
            else b.AlignWith(a);
        }

        private static int PrecisionFor(ref BigDecimal a, ref BigDecimal b)
        {
            if (a.UsingDefaultPrecision == b.UsingDefaultPrecision) return a.Precision > b.Precision ? a.Precision : b.Precision;
            else return a.UsingDefaultPrecision ? b.Precision : a.Precision;
        }

        /// <summary>
        /// Returns a BigDecimal with the specified precision.
        /// </summary>
        /// <param name="precision">The precision.</param>
        /// <returns>A BigDecimal with the specified precision.</returns>
        public BigDecimal WithPrecision(int precision)
        {
            Contract.Requires(precision >= 0);
            return new BigDecimal(this, precision);
        }

        /// <summary>
        /// Object.GetHashCode();
        /// </summary>
        /// <returns>The hash code of the current instance.</returns>
        public override int GetHashCode()
        {
            return Util.Hash(this.Mantissa, this.Exponent);
        }

        private string _ToStringCache;

        /// <summary>
        /// Object.ToString()
        /// </summary>
        /// <returns>The string representation of the BigDecimal.</returns>
        public override string ToString()
        {
            if (_ToStringCache != null) return _ToStringCache;

            BigDecimal withoutExtra = this.WithoutExtraPrecision();
            bool positive = withoutExtra.Mantissa >= 0;
            StringBuilder sb = new StringBuilder((positive ? withoutExtra.Mantissa : -withoutExtra.Mantissa).ToString());

            int exp = withoutExtra.Exponent,
                index = sb.Length + exp;

            if (index > sb.Length)
            {
                sb.Append('0', index - sb.Length);
            }
            else if (index <= 0)
            {
                sb.Insert(0, "0", -index + 1);
                index = 1;
            }

            bool comma = false;
            if (index < sb.Length)
            {
                sb.Insert(index, '.');
                comma = true;
            }

            if (!positive) sb.Insert(0, CultureInfo.CurrentCulture.NumberFormat.NegativeSign);
            string res = sb.ToString();
            if (comma) res = res.TrimEnd('0');
            if (res[res.Length - 1] == '.') res = res.Substring(0, res.Length - 1);
            return _ToStringCache = res;
        }

        #endregion Other

        /*
        * myung_ari
        * TryParse
        */
        public static bool TryParse(string value, out BigDecimal result)
        {
            result = Parse(value);
            return true;
        }
    }
}



// https://gist.github.com/nberardi/2667136
/*
using System;
using System.Linq;

namespace System.Numerics
{
	public struct BigDecimal : IConvertible, IFormattable, IComparable, IComparable<BigDecimal>, IEquatable<BigDecimal>
	{
		public static readonly BigDecimal MinusOne = new BigDecimal(BigInteger.MinusOne, 0);
		public static readonly BigDecimal Zero = new BigDecimal(BigInteger.Zero, 0);
		public static readonly BigDecimal One = new BigDecimal(BigInteger.One, 0);

		private readonly BigInteger _unscaledValue;
		private readonly int _scale;

		public BigDecimal(double value)
			: this((decimal)value) { }

		public BigDecimal(float value)
			: this((decimal)value) { }

		public BigDecimal(decimal value)
		{
			var bytes = FromDecimal(value);

			var unscaledValueBytes = new byte[12];
			Array.Copy(bytes, unscaledValueBytes, unscaledValueBytes.Length);

			var unscaledValue = new BigInteger(unscaledValueBytes);
			var scale = bytes[14];

			if (bytes[15] == 128)
				unscaledValue *= BigInteger.MinusOne;

			_unscaledValue = unscaledValue;
			_scale = scale;
		}

		public BigDecimal(int value)
			: this(new BigInteger(value), 0) { }

		public BigDecimal(long value)
			: this(new BigInteger(value), 0) { }

		public BigDecimal(uint value)
			: this(new BigInteger(value), 0) { }

		public BigDecimal(ulong value)
			: this(new BigInteger(value), 0) { }

		public BigDecimal(BigInteger unscaledValue, int scale)
		{
			_unscaledValue = unscaledValue;
			_scale = scale;
		}

		public BigDecimal(byte[] value)
		{
			byte[] number = new byte[value.Length - 4];
			byte[] flags = new byte[4];

			Array.Copy(value, 0, number, 0, number.Length);
			Array.Copy(value, value.Length - 4, flags, 0, 4);

			_unscaledValue = new BigInteger(number);
			_scale = BitConverter.ToInt32(flags, 0);
		}

		public bool IsEven { get { return _unscaledValue.IsEven; } }
		public bool IsOne { get { return _unscaledValue.IsOne; } }
		public bool IsPowerOfTwo { get { return _unscaledValue.IsPowerOfTwo; } }
		public bool IsZero { get { return _unscaledValue.IsZero; } }
		public int Sign { get { return _unscaledValue.Sign; } }

		public override string ToString()
		{
			var number = _unscaledValue.ToString("G");

			if (_scale > 0)
				return number.Insert(number.Length - _scale, ".");

			return number;
		}

		public byte[] ToByteArray()
		{
			var unscaledValue = _unscaledValue.ToByteArray();
			var scale = BitConverter.GetBytes(_scale);

			var bytes = new byte[unscaledValue.Length + scale.Length];
			Array.Copy(unscaledValue, 0, bytes, 0, unscaledValue.Length);
			Array.Copy(scale, 0, bytes, unscaledValue.Length, scale.Length);

			return bytes;
		}

		private static byte[] FromDecimal(decimal d)
		{
			byte[] bytes = new byte[16];

			int[] bits = decimal.GetBits(d);
			int lo = bits[0];
			int mid = bits[1];
			int hi = bits[2];
			int flags = bits[3];

			bytes[0] = (byte)lo;
			bytes[1] = (byte)(lo >> 8);
			bytes[2] = (byte)(lo >> 0x10);
			bytes[3] = (byte)(lo >> 0x18);
			bytes[4] = (byte)mid;
			bytes[5] = (byte)(mid >> 8);
			bytes[6] = (byte)(mid >> 0x10);
			bytes[7] = (byte)(mid >> 0x18);
			bytes[8] = (byte)hi;
			bytes[9] = (byte)(hi >> 8);
			bytes[10] = (byte)(hi >> 0x10);
			bytes[11] = (byte)(hi >> 0x18);
			bytes[12] = (byte)flags;
			bytes[13] = (byte)(flags >> 8);
			bytes[14] = (byte)(flags >> 0x10);
			bytes[15] = (byte)(flags >> 0x18);

			return bytes;
		}

		#region Operators

		public static bool operator ==(BigDecimal left, BigDecimal right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BigDecimal left, BigDecimal right)
		{
			return !left.Equals(right);
		}

		public static bool operator >(BigDecimal left, BigDecimal right)
		{
			return (left.CompareTo(right) > 0);
		}

		public static bool operator >=(BigDecimal left, BigDecimal right)
		{
			return (left.CompareTo(right) >= 0);
		}

		public static bool operator <(BigDecimal left, BigDecimal right)
		{
			return (left.CompareTo(right) < 0);
		}

		public static bool operator <=(BigDecimal left, BigDecimal right)
		{
			return (left.CompareTo(right) <= 0);
		}

		public static bool operator ==(BigDecimal left, decimal right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(BigDecimal left, decimal right)
		{
			return !left.Equals(right);
		}

		public static bool operator >(BigDecimal left, decimal right)
		{
			return (left.CompareTo(right) > 0);
		}

		public static bool operator >=(BigDecimal left, decimal right)
		{
			return (left.CompareTo(right) >= 0);
		}

		public static bool operator <(BigDecimal left, decimal right)
		{
			return (left.CompareTo(right) < 0);
		}

		public static bool operator <=(BigDecimal left, decimal right)
		{
			return (left.CompareTo(right) <= 0);
		}

		public static bool operator ==(decimal left, BigDecimal right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(decimal left, BigDecimal right)
		{
			return !left.Equals(right);
		}

		public static bool operator >(decimal left, BigDecimal right)
		{
			return (left.CompareTo(right) > 0);
		}

		public static bool operator >=(decimal left, BigDecimal right)
		{
			return (left.CompareTo(right) >= 0);
		}

		public static bool operator <(decimal left, BigDecimal right)
		{
			return (left.CompareTo(right) < 0);
		}

		public static bool operator <=(decimal left, BigDecimal right)
		{
			return (left.CompareTo(right) <= 0);
		}

		#endregion

		#region Explicity and Implicit Casts

		public static explicit operator byte(BigDecimal value) { return value.ToType<byte>(); }
		public static explicit operator sbyte(BigDecimal value) { return value.ToType<sbyte>(); }
		public static explicit operator short(BigDecimal value) { return value.ToType<short>(); }
		public static explicit operator int(BigDecimal value) { return value.ToType<int>(); }
		public static explicit operator long(BigDecimal value) { return value.ToType<long>(); }
		public static explicit operator ushort(BigDecimal value) { return value.ToType<ushort>(); }
		public static explicit operator uint(BigDecimal value) { return value.ToType<uint>(); }
		public static explicit operator ulong(BigDecimal value) { return value.ToType<ulong>(); }
		public static explicit operator float(BigDecimal value) { return value.ToType<float>(); }
		public static explicit operator double(BigDecimal value) { return value.ToType<double>(); }
		public static explicit operator decimal(BigDecimal value) { return value.ToType<decimal>(); }
		public static explicit operator BigInteger(BigDecimal value)
		{
			var scaleDivisor = BigInteger.Pow(new BigInteger(10), value._scale);
			var scaledValue = BigInteger.Divide(value._unscaledValue, scaleDivisor);
			return scaledValue;
		}

		public static implicit operator BigDecimal(byte value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(sbyte value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(short value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(int value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(long value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(ushort value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(uint value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(ulong value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(float value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(double value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(decimal value) { return new BigDecimal(value); }
		public static implicit operator BigDecimal(BigInteger value) { return new BigDecimal(value, 0); }

		#endregion

		public T ToType<T>() where T : struct
		{
			return (T)((IConvertible)this).ToType(typeof(T), null);
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			var scaleDivisor = BigInteger.Pow(new BigInteger(10), this._scale);
			var remainder = BigInteger.Remainder(this._unscaledValue, scaleDivisor);
			var scaledValue = BigInteger.Divide(this._unscaledValue, scaleDivisor);

			if (scaledValue > new BigInteger(Decimal.MaxValue))
				throw new ArgumentOutOfRangeException("value", "The value " + this._unscaledValue + " cannot fit into " + conversionType.Name + ".");

			var leftOfDecimal = (decimal)scaledValue;
			var rightOfDecimal = ((decimal)remainder) / ((decimal)scaleDivisor);

			var value = leftOfDecimal + rightOfDecimal;
			return Convert.ChangeType(value, conversionType);
		}

		public override bool Equals(object obj)
		{
			return ((obj is BigDecimal) && Equals((BigDecimal)obj));
		}

		public override int GetHashCode()
		{
			return _unscaledValue.GetHashCode() ^ _scale.GetHashCode();
		}

		#region IConvertible Members

		TypeCode IConvertible.GetTypeCode()
		{
			return TypeCode.Object;
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot cast BigDecimal to Char");
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException("Cannot cast BigDecimal to DateTime");
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		string IConvertible.ToString(IFormatProvider provider)
		{
			return Convert.ToString(this);
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		#endregion

		#region IFormattable Members

		public string ToString(string format, IFormatProvider formatProvider)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;

			if (!(obj is BigDecimal))
				throw new ArgumentException("Compare to object must be a BigDecimal", "obj");

			return CompareTo((BigDecimal)obj);
		}

		#endregion

		#region IComparable<BigDecimal> Members

		public int CompareTo(BigDecimal other)
		{
			var unscaledValueCompare = this._unscaledValue.CompareTo(other._unscaledValue);
			var scaleCompare = this._scale.CompareTo(other._scale);

			// if both are the same value, return the value
			if (unscaledValueCompare == scaleCompare)
				return unscaledValueCompare;

			// if the scales are both the same return unscaled value
			if (scaleCompare == 0)
				return unscaledValueCompare;

			var scaledValue = BigInteger.Divide(this._unscaledValue, BigInteger.Pow(new BigInteger(10), this._scale));
			var otherScaledValue = BigInteger.Divide(other._unscaledValue, BigInteger.Pow(new BigInteger(10), other._scale));

			return scaledValue.CompareTo(otherScaledValue);
		}

		#endregion

		#region IEquatable<BigDecimal> Members

		public bool Equals(BigDecimal other)
		{
			return this._scale == other._scale && this._unscaledValue == other._unscaledValue;
		}

		#endregion
	}
}
*/