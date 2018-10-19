using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayfairCipher_윤주혜
{

    public partial class Form1 : Form
    {
        string key;
        char[,] panel;
        string text;
        List<char> key_list;
        List<char> text_list;
        List<int> x;
        int element;
        bool check;
        char[] char_encryption;
        List<char> encryption_list;

        public Form1()
        {
            InitializeComponent();
            this.Text = "PlayfairCipher_윤주혜";
        }         

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                /*키 입력*/
                key = textBox1.Text;
                char[] char_key = key.ToCharArray();//char 배열로 변환

                /*key_list 리스트에 입력 받은 key값 넣기*/
                key_list = new List<char>(char_key);

                /*key_list 리스트에 알파벳 넣기*/
                for (char alpha = 'a'; alpha <= 'y'; alpha++)
                {
                    key_list.Add(char_key[97 - 'a'] = alpha);
                }

                /*key_list 리스트의 모든 중복값 제거*/
                key_list = key_list.Distinct().ToList();

                /* 5*5 panel 배열에 key_list 리스트에 들어있는 값 넣기 */
                panel = new char[5, 5];
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        panel[i, j] = key_list[i * 5 + j];
                    }
                }
            }
            catch
            {
                MessageBox.Show("다시 입력해주세요");
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                /*평문 입력*/
                text = textBox2.Text;
                char[] char_text = text.Replace(" ", "").ToCharArray();//공백 제거하고 c

                /*text_list 리스트에 입력받은 text값 넣기*/
                text_list = new List<char>(char_text);

                x = new List<int>();
                element = 0;
                check = false;

                /*짝수(i), 홀수번째(i+1) 비교해서 중복값 있으면 x넣기 */
                for (int i = 0; i < text_list.Count - 1; i++)
                {
                    if (i % 2 == 0)
                    {
                        if (text_list[i] == text_list[i + 1])
                        {
                            text_list.Insert(i + 1, 'x');

                            /*x를 Remove할 때마다 글자 길이가 달라지기 때문에 element 변수에 넣어서 -1씩 해줌*/
                            element = i + 1;
                            if (x.Count > 0) element -= 1;

                            /*x를 넣은 위치를 기억하기 위해 x 리스트에 추가*/
                            x.Add(element);
                        }
                    }
                }
                /*text_list 리스트 길이가 홀수일때 마지막 부분에 x넣기*/
                if (text_list.Count % 2 == 1)
                {
                    text_list.Insert(text_list.Count, 'x');

                    /*홀수일 때 x를 넣었는지 확인*/
                    check = true;
                }
            }
            catch
            {
                MessageBox.Show("다시 입력해주세요");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                /*암호문을 넣을 배열*/
                char_encryption = new char[text_list.Count];

                /*text_list 리스트 요소로 key_list의 같은 요소의 인덱스 구하기*/
                for (int a = 0; a < text_list.Count - 1; a++)
                {
                    if (a % 2 == 0)
                    {
                        /*짝수번째 text_list 리스트의 리스트의 인덱스 (0,2,4,6...)*/
                        int i1 = key_list.IndexOf(text_list[a]) / 5;
                        int j1 = key_list.IndexOf(text_list[a]) % 5;

                        /*홀수번째 text_list 리스트의 리스트의 인덱스 (1,3,5,7...)*/
                        int i2 = key_list.IndexOf(text_list[a + 1]) / 5;
                        int j2 = key_list.IndexOf(text_list[a + 1]) % 5;

                        /*i가 다르고 j가 다를때 i를 다른 요소의 i값으로 교체*/
                        if (i1 != i2 && j1 != j2)
                        {
                            int temp;
                            temp = i1;
                            i1 = i2;
                            i2 = temp;
                        }
                        /*j가 같고 i가 다를때 밑으로 단, 5일때 인덱스는 0*/
                        if (i1 != i2 && j1 == j2)
                        {
                            i1 += 1;
                            i2 += 1;
                            if (i1 == 5)
                            {
                                i1 = 0;
                            }
                            if (i2 == 5)
                            {
                                i2 = 0;
                            }
                        }
                        /*i가 같고 j가다를때 오른쪽으로 단, 5일때 인덱스는 0*/
                        if (i1 == i2 && j1 != j2)
                        {
                            j1 += 1;
                            j2 += 1;
                            if (j1 == 5)
                            {
                                j1 = 0;
                            }
                            if (j2 == 5)
                            {
                                j2 = 0;
                            }
                        }
                        /*char_encryption 배열에 암호 넣기*/
                        char_encryption[a] = panel[i1, j1];
                        char_encryption[a + 1] = panel[i2, j2];
                    }
                }
                /*encryption_list 리스트에 char_encryption 넣기*/
                encryption_list = new List<char>(char_encryption);

                /*암호문 초기화*/
                textBox3.Text = "";

                /*encryption_list 리스트 출력*/
                foreach (char i in encryption_list)
                {
                    textBox3.Text += i;
                }
            }
            catch
            {
                MessageBox.Show("키와 평문을 입력해주세요");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                /*char_decryption 배열*/
                char[] char_decryption = new char[encryption_list.Count];

                /*복호하기*/
                for (int a = 0; a < encryption_list.Count - 1; a++)
                {
                    if (a % 2 == 0)
                    {
                        int i1 = key_list.IndexOf(char_encryption[a]) / 5;
                        int j1 = key_list.IndexOf(char_encryption[a]) % 5;

                        int i2 = key_list.IndexOf(char_encryption[a + 1]) / 5;
                        int j2 = key_list.IndexOf(char_encryption[a + 1]) % 5;

                        /*i가 다르고 j가 다를때 i를 다른 요소의 i값으로 교체*/
                        if (i1 != i2 && j1 != j2)
                        {
                            int temp;
                            temp = i1;
                            i1 = i2;
                            i2 = temp;
                        }
                        /*j가 같고 i가 다를때 위로 단, -1일때 인덱스는 4*/
                        if (i1 != i2 && j1 == j2)
                        {
                            i1 -= 1;
                            i2 -= 1;
                            if (i1 == -1)
                            {
                                i1 = 4;
                            }
                            if (i2 == -1)
                            {
                                i2 = 4;
                            }
                        }
                        /*i가 같고 j가 다를때 왼쪽으로 단, -1일때 인덱스는 4*/
                        if (i1 == i2 && j1 != j2)
                        {
                            j1 -= 1;
                            j2 -= 1;
                            if (j1 == -1)
                            {
                                j1 = 4;
                            }
                            if (j2 == -1)
                            {
                                j2 = 4;
                            }
                        }
                        /*char_decryption 배열에 복호 넣기*/
                        char_decryption[a] = panel[i1, j1];
                        char_decryption[a + 1] = panel[i2, j2];
                    }
                }
                /*decryption_list 리스트에 받은 char_decryption 넣기*/
                List<char> decryption_list = new List<char>(char_decryption);

                /*x값 위치를 저장한 x 리스트로 제거*/
                for (int i = 0; i < x.Count; i++)
                {
                    decryption_list.RemoveAt(x[i]);
                }

                /*check가 true일때(홀수여서 x를 붙인 상태) 마지막 요소(x) 삭제*/
                if (check)
                {
                    decryption_list.RemoveAt(decryption_list.Count - 1);
                }

                /*복호문 초기화*/
                textBox4.Text = "";
                
                /*복호문 출력*/
                foreach (char i in decryption_list)
                {
                    textBox4.Text += i;
                }
            }
            catch
            {
                MessageBox.Show("키와 평문을 입력하고 암호화버튼을 먼저 눌러주세요");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                /*label 초기화*/
                label5.Text = "";
                label6.Text = "";
                label7.Text = "";
                label8.Text = "";
                label9.Text = "";

                label10.Text = "";
                label11.Text = "";
                label12.Text = "";
                label13.Text = "";
                label14.Text = "";

                label15.Text = "";
                label16.Text = "";
                label17.Text = "";
                label18.Text = "";
                label19.Text = "";

                label20.Text = "";
                label21.Text = "";
                label22.Text = "";
                label23.Text = "";
                label24.Text = "";

                label25.Text = "";
                label26.Text = "";
                label27.Text = "";
                label28.Text = "";
                label29.Text = "";

                /*label에 panel 넣음*/
                label5.Text += panel[0, 0];
                label6.Text += panel[0, 1];
                label7.Text += panel[0, 2];
                label8.Text += panel[0, 3];
                label9.Text += panel[0, 4];

                label10.Text += panel[1, 0];
                label11.Text += panel[1, 1];
                label12.Text += panel[1, 2];
                label13.Text += panel[1, 3];
                label14.Text += panel[1, 4];

                label15.Text += panel[2, 0];
                label16.Text += panel[2, 1];
                label17.Text += panel[2, 2];
                label18.Text += panel[2, 3];
                label19.Text += panel[2, 4];

                label20.Text += panel[3, 0];
                label21.Text += panel[3, 1];
                label22.Text += panel[3, 2];
                label23.Text += panel[3, 3];
                label24.Text += panel[3, 4];

                label25.Text += panel[4, 0];
                label26.Text += panel[4, 1];
                label27.Text += panel[4, 2];
                label28.Text += panel[4, 3];
                label29.Text += panel[4, 4];
            }
            catch
            {
                MessageBox.Show("키를 입력해주세요");
            }
        }
    }
}
