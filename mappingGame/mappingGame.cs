using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mappingGame
{
    public partial class mappingGame : Form
    {
        public mappingGame()
        {
            InitializeComponent();
           
        }
        //Rastgele sayı üretmek için random sınıfından nesne üretilir
        Random rnd = new Random();

        //Butonlara resim olacak ikonları dinamik dizide tutma
        ArrayList icons =new ArrayList  { "!", "!", ",", "," ,"F","H","B","G","K","S","F","H","B","G","K","S"};
        
        //<summary>
        //Oyna butonuna basma olayı
        //</summary>
        int iconIndex;
        private void startBtn_Click(object sender, EventArgs e)
        {
            //Formu oyun için düzenleme
            startBtn.Visible = false;
            userPnl.Visible = true;
            //En başta resimlerin görüneceği süreyi çağırma
            startingTime.Start();
            //Formdaki nesneleri kontrol ettirip
            foreach (Control item in Controls)
            {
                //Oyna butonu hariç butonlara ulaşıp
                if (item is Button && item != startBtn)
                {
                    item.Enabled = true;
                    //Rastgele ikonlar atama
                    iconIndex = rnd.Next(icons.Count);
                    item.Text = icons[iconIndex].ToString();
                    //Atanan ikonu listeden silme, ikiden fazla ya da az atanan olmasın diye
                    icons.RemoveAt(iconIndex);
                }

            }

        }

        //<summary>
        //Skor belirleme olayı
        //</summary>
        public void score(string iconText, Label userScore)
        {
            //Her çağrıldığında sıfıranır üstüne tekrar eklemesin diye
            int controlScore =0;
            foreach (Control item in Controls)
            {
                //Girilen ikon metni kaç defa varsa o kadar oluyor tuttuğumuz değişken
                if (item is Button && item.Text == iconText)
                    controlScore++;
            } 
            //10 ile çağrılıp kaçıncı kullanıcıysa girilen labela göre skoruna yazma
            userScore.Text = Convert.ToString(controlScore * 10);
            
        }

        //<summary>
        //Eşleştirme yapılan butonlardan herhangi birine tıklama olayı
        //</summary>
        //Tıklanan iki butonu tutmak için değişkenler atanır
        Button firstClick, secondClick;
        //Kullanıcının kaçıncı oyuncu olduğunu kontrol etmek için bir değişken belirlenir
        int controlUser = 0;
        private void Buton_Click(object sender, EventArgs e)
        {  
            //Kullanıcıya verilen süre başlatılır
            mappingTime.Enabled = true;
            //Tıklanan buton önce bir değişkende tutulur
            Button btn = sender as Button;
            //Sonra ihtiyacımız olan iki butona atanır
            if (firstClick == null)
            {
                firstClick = btn;
                //Tıklanan butonların ikonlarını gösterme
                firstClick.ForeColor = Color.Black;
                firstClick.Enabled = false;
                return;
            }
            else
            {
            secondClick= btn ;
            secondClick.ForeColor = Color.Black;

            }
            //Eşleştirme doğruysa
            if(firstClick.Text==secondClick.Text)
            {
                //Birinci oyuncuysa ikonlar tik olur
                if (controlUser == 0)
                {
                    firstClick.Text = "a";
                    secondClick.Text = "a";  
                }
                else
                {
                    //İkinci oyuncuysa çarpı olur
                    firstClick.Text = "r";
                    secondClick.Text = "r";
                }
              
                secondClick.Enabled = false;
                firstClick = null;
                secondClick = null;
                timeLbl.Text = "6";
            }
            else 
            {
                firstClick.Enabled = true;
                //Yanlış eşleştirme yapılırsa tıklanan butonların ikonları gösterme süresi olayı çağrılır
                showTime.Start();
                
                if (controlUser == 0)
                {
                    //Birinci oyuncuysa onun için skor methodu çağrılır
                    score("a", firstScorelBL);
                    controlUser++;
                }
                else
                {
                    //ikinciyse ikinci oyuncu için skor methodu çağrılır
                    score("r", secondScoreLbl);
                    controlUser = 0;
                }
                //Eşleştirme süresine erişim durdurulur
                mappingTime.Enabled = false;
                if (MessageBox.Show("Sıra diğer oyuncuda!", "Sıran Geçti", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    //Mesaj kutusunda tamama tıklandıktan sonra tekrar eşleştirme süresi olayı çağrılır
                    mappingTime.Start();
                    timeLbl.Text = "6";
                }


            }

            //Butonların hepsinin eşleştirilip eşleştirilmediği kontrol edilirken son kontrole gelinmiş mi diye dönme sayısını bir değişken de tutma
            int indeks = 0;
            foreach (Control item in Controls)
            {

                if (item is Button && item != startBtn)
                {
                    indeks++;
                    //Butonların ikonlarını temsil eden textleri kontrol edilir
                    if (item.Text!="a"&&item.Text!="r")
                    {
                        //Eğer eşleştirme ikonundan farklı bir ikonsa döngü kesilir
                        break;
                    }
                    else if(indeks==16)
                    {
                        //Hepsi eşleştirme ikonlarından biriyse ve son kontrele gelindiyse oyun bitti demektir skorlar alınır
                        if (controlUser == 0)
                        {
                            score("a", firstScorelBL);
                            controlUser++;
                        }
                        else
                        {
                            score("r", secondScoreLbl);
                            controlUser = 0;
                        }
                        mappingTime.Enabled = false;
                        //Skorlara göre kazanan açıklanır
                        if (Convert.ToInt32(firstScorelBL.Text) < Convert.ToInt32(secondScoreLbl.Text))
                            MessageBox.Show("Oyun bitti ikinci oyuncu kazandı!");
                        else if (Convert.ToInt32(firstScorelBL.Text) > Convert.ToInt32(secondScoreLbl.Text))
                            MessageBox.Show("Oyun bitti birinci oyuncu kazandı!");
                        else
                            MessageBox.Show("Oyun bitti berabere!");

                        //Form yeni oyun için düzenlenir
                        userPnl.Visible = false;
                        startBtn.Visible = true;
                        firstScorelBL.Text = "";
                        secondScoreLbl.Text="";
                        mappingTime.Enabled = false;
                        //Kullanıcı kontrolu sıfırlanır birinci oyuncu başlasın diye
                        controlUser = 0;
                        //DİZİ elemanları tekrar eklenir
                        icons = new ArrayList { "!", "!", ",", ",", "F", "H", "B", "G", "K", "S", "F", "H", "B", "G", "K", "S" };
                        
                    }

                }
            }

        }

        //<summary>
        //Resimlerin oyun başında gözükme süresi olayı
        //</summary>
        private void startingTime_Tick(object sender, EventArgs e)
        {
            startingTime.Stop();
            //Bittiğinde butonları beyaz gösterme
            foreach (Control item in Controls)
            {
                if (item is Button && item != startBtn)
                {
                    item.ForeColor = Color.White;
                }
            }
        }

        //<summary>
        //Yanlış eşleştirme de butonların ikonlarını gösterir vaziyette kalma süresi olayı
        //</summary>
        private void showTime_Tick(object sender, EventArgs e)
        {
            showTime.Stop();
            //Bittiğinde butonları eski haline döner
            firstClick.ForeColor = Color.White;
            secondClick.ForeColor = Color.White;
            //Değişkenleri temizleme çünkü yeni tıklanan butonlar atanacak
            firstClick = null;
            secondClick = null;
        }

        //<summary>
        //Eşleştirme yapması için kullanıcıya verilen süre olayı
        //</summary>
        private void mappingTime_Tick(object sender, EventArgs e)
        {
            //Sürenin yazdığı textboxı geriye doğru saydırma
            timeLbl.Text = (Convert.ToInt32(timeLbl.Text) - 1).ToString();
            //Biterse sırayı diğer oyuncuya geçirme
            if (Convert.ToInt32(timeLbl.Text) == 0)
            {
                
                //Süre durdurulur
                mappingTime.Stop();
                //Tek buton Basılı kaldıysa düzeltme
                foreach (Control item in Controls)
                {
                    if (item is Button && item != startBtn)
                    {
                        if (item.Text != "a" && item.Text != "r" && item.Enabled == false)
                        {
                            item.Enabled = true;
                            item.ForeColor =Color.White;
                            firstClick = null;
                        }
                       

                    }
                }
                if (MessageBox.Show("Sıra diğer oyuncuda!", "Sıran Geçti", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    //Mesaj kutusuna tamam denildikten sonra tekrar başlar
                    mappingTime.Start();
                    timeLbl.Text = "6";
                    //Birinci veya ikinci oyuncuya göre skor methodu çağrılır
                    if (controlUser == 0)
                    {
                        score("a", firstScorelBL);
                        controlUser++;
                    }
                    else
                    {
                        score("r", secondScoreLbl);
                        controlUser = 0;
                    }
                }

            }
        }
         
    }
}

