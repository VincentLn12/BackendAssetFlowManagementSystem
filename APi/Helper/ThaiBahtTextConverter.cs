public static class ThaiBahtTextConverter
{
    private static readonly string[] NumberText =
    {
        "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่",
        "ห้า", "หก", "เจ็ด", "แปด", "เก้า"
    };

    private static readonly string[] PositionText =
    {
        "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน"
    };

    public static string ToThaiBahtText(decimal amount)
    {
        if (amount == 0)
            return "ศูนย์บาทถ้วน";

        string[] parts = amount.ToString("0.00").Split('.');
        string baht = ConvertInteger(parts[0]);
        string satang = ConvertInteger(parts[1]);

        if (parts[1] == "00")
            return baht + "บาทถ้วน";

        return baht + "บาท" + satang + "สตางค์";
    }

    private static string ConvertInteger(string number)
    {
        if (number == "0")
            return "";

        string result = "";
        int len = number.Length;

        for (int i = 0; i < len; i++)
        {
            int digit = int.Parse(number[i].ToString());
            int pos = len - i - 1;

            if (digit == 0)
                continue;

            if (pos == 0)
            {
                if (digit == 1 && len > 1)
                    result += "เอ็ด";
                else
                    result += NumberText[digit];
            }
            else if (pos == 1)
            {
                if (digit == 1)
                    result += "สิบ";
                else if (digit == 2)
                    result += "ยี่สิบ";
                else
                    result += NumberText[digit] + "สิบ";
            }
            else
            {
                result += NumberText[digit] + PositionText[pos % 6];

                if (pos == 6)
                    result += "ล้าน";
            }
        }

        return result;
    }
}