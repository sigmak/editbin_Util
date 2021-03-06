﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;


//출처: https://ilbbang.tistory.com/entry/폴더-내-파일-목록-가져오기하위폴더-포함여부
namespace editbin_Util
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox2.ReadOnly = true;//읽기 전용

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            CommonOpenFileDialog dialog = new CommonOpenFileDialog(); // 새로운 폴더 선택 Dialog 를 생성합니다.
            dialog.IsFolderPicker = true; //
            //dialog.Filters. = "엑셀 파일 (*.xls)|*.xls|엑셀 파일 (*.xlsx)|*.xlsx";
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok) // 폴더 선택이 정상적으로 되면 아래 코드를 실행합니다.
            {
                label2.Text = dialog.FileName; // 선택한 폴더 이름을 label2에 출력합니다.
                DataTable dt_filelistinfo = GetFileListFromFolderPath(dialog.FileName);
                ShowDataFromDataTableToDataGridView(dt_filelistinfo, dataGridView1);
            }
            */
            label2.Text = Application.StartupPath + @"\tmp1"; // 프로그램 실행폴더의 하위 폴더인 tmp1 폴더 지정
            DataTable dt_filelistinfo = GetFileListFromFolderPath(label2.Text);
            ShowDataFromDataTableToDataGridView(dt_filelistinfo, dataGridView1);

        }

        /// <summary>
        /// 선택한 폴더의 파일 목록을 DataTable 형식으로 내보냅니다.
        /// </summary>
        /// <param name="FolerName">선택한 폴더의 전체 경로를 입력합니다.</param>
        /// <retruns>DataTable</retruns>
        private DataTable GetFileListFromFolderPath(string FolderName)
        {
            DirectoryInfo di = new DirectoryInfo(FolderName);  // 해당 폴더 정보를 가져옵니다.

            DataTable dt1 = new DataTable(); // 새로운 테이블을 작성합니다. (FileINfo 에서 가져오기 원하는 속성을 열로 추가합니다.)
            dt1.Columns.Add("No", typeof(int)); // No 행숫자표시
            dt1.Columns.Add("Folder", typeof(string)); //파일의 폴더
            dt1.Columns.Add("FileName", typeof(string)); //파일 이름(확장자 포함)
            dt1.Columns.Add("Extension", typeof(string)); //확장자
            dt1.Columns.Add("CreationTime", typeof(DateTime)); //생성 일자
            dt1.Columns.Add("LastWriteTime", typeof(DateTime)); //마지막 수정 일자
            dt1.Columns.Add("LastAccessTime", typeof(DateTime)); //마지막 접근 일자
            int no = 0;
            foreach(FileInfo fFile in di.GetFiles()) //선택 폴더의 파일 목록을 스캔합니다.
            {
                if (fFile.Extension.ToUpper().Equals(".EXE")) //확장자가 대문자 치환해서 .EXE 인 경우 
                {
                    no++;
                    dt1.Rows.Add(no, fFile.DirectoryName, fFile.Name, fFile.Extension, fFile.CreationTime, fFile.LastWriteTime, fFile.LastAccessTime); // 개별 파일 별로 정보를 추가합니다.
                    
                }

            }

            /*
            if (checkbox1.Checked == true) //하위 폴더 포함 될 경우
            {
                DirectoryInfo[] di_sub = di.GetDirectories(); // 하위 폴더 목록들의 정보를 가져옵니다.
                foreach (DirectoryInfo di1 in di_sub) // 하위 폴더모록을 스캔합니다.
                {
                    dt1.Rows.Add(File.DirectoryName, File.Name, File.Extension, File.CreationTime, File.LastWriteTime, File.LastAccessTime); // 개별 파일 별로 정보를 추가합니다.
                }
            }
            */

            return dt1;
        }

        /// <summary>
        /// 선택한 폴더의 파일 목록을 가져와서 DataGridView 도구에 보여줍니다.
        /// </summary>
        /// <param name="dt1"">선택한 폴더의 파일 목록이 들어 있는 DataTable을 입력합니다.</param>
        /// <param name="dgv1"">결과를 출력할 DataGridView를 선택합니다.</param>
        private void ShowDataFromDataTableToDataGridView(DataTable dt1, DataGridView dgv1)
        {
            dgv1.Rows.Clear(); // 이전 정보가 있을 경우, 모든 행을 삭제합니다.
            dgv1.Columns.Clear(); // 이전 정보가 있을 경우, 모든 열을 삭제합니다.

            foreach (DataColumn dc1 in dt1.Columns) // 선택한 파일 목록이 들어 있는 DataTable의 모든 열을 스캔합니다.
            {
                dgv1.Columns.Add(dc1.ColumnName, dc1.ColumnName); //출력할 DataGridView에 열을 추가합니다.
            }

            int row_index = 0; // 행 인덱스 번호(초기 값)
            foreach(DataRow dr1 in dt1.Rows) //선택한 파일 목록이 들어 있는 DataTable 의 모든 행을 스캔합니다.
            {
                dgv1.Rows.Add(); // 빈 행을 하나 추가합니다.
                foreach (DataColumn dc1 in dt1.Columns) // 선택한 파일 목록이 들어있는 DataTable의 모든 열을 스캔합니다.
                {
                    dgv1.Rows[row_index].Cells[dc1.ColumnName].Value = dr1[dc1.ColumnName]; // 선택 행 별로, 스캔하는 열에 해당하는 셀 값을 입력합니다.
                }
                row_index++; // 다음 행 인덱스를 선택하기 위해 1을 더해줍니다.
            }

            foreach(DataGridViewColumn drvc1 in dgv1.Columns) // 결괄르 출력할 DataGridView의 모든 열을 스캔합니다.
            {
                drvc1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // 선택 열의 너비를 자동으로 설정합니다.
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;// 수정못하게 읽기전용으로
            dataGridView1.MultiSelect = false; //여러개의 셀이나 행을 선택하지 못하도록 막고 싶을 때
            dataGridView1.AllowUserToAddRows = false;  // 빈레코드 표시 안하기

            dataGridView2.ReadOnly = true;// 수정못하게 읽기전용으로
            dataGridView2.MultiSelect = false; //여러개의 셀이나 행을 선택하지 못하도록 막고 싶을 때
            dataGridView2.AllowUserToAddRows = false;  // 빈레코드 표시 안하기

            dataGridView3.ReadOnly = true;// 수정못하게 읽기전용으로
            dataGridView3.MultiSelect = false; //여러개의 셀이나 행을 선택하지 못하도록 막고 싶을 때
            dataGridView3.AllowUserToAddRows = false;  // 빈레코드 표시 안하기


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //label2.Text = Application.StartupPath + @"\exefix"; // 프로그램 실행폴더의 하위 폴더인 tmp1 폴더 지정
            if (label2.Text.Equals("없음"))
            {
                return;// 은 함수 자체를 탈출
            }
            string exefixFolder = Application.StartupPath + @"\exefix";

            // 출처: https://link2me.tistory.com/786
            // 셀 내용 읽기 1행, 0열  --> 행은 Rows, 열은 Column 이 아니라 Cells 로 표기하네
            // dataGridView1.Rows[1].Cells[0].Value.ToString();

            // 출처: https://stackoverflow.com/questions/19737436/looping-each-row-in-datagridview
            // foreach (DataGridViewRow row in datagridviews.Rows)
            // {
            //    currQty += row.Cells["qty"].Value;
            //    //More code here
            // }
            textBox2.Text = "";

            /*
                        // Start the child process.
                        Process p = new Process();
                        // Redirect the output stream of the child process.
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.FileName = "YOURBATCHFILE.bat";
                        p.Start();
                        // Do not wait for the child process to exit before
                        // reading to the end of its redirected stream.
                        // p.WaitForExit();
                        // Read the output stream first and then wait.
                        string output = p.StandardOutput.ReadToEnd();
                        p.WaitForExit();
            */
            
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                //dt1.Columns.Add("Folder", typeof(string)); //파일의 폴더
                //dt1.Columns.Add("FileName", typeof(string)); //파일 이름(확장자 포함) 

                //참고 : https://frontjang.info/entry/C%EC%97%90%EC%84%9C-%EC%99%B8%EB%B6%80%ED%94%84%EB%A1%9C%EA%B7%B8%EB%9E%A8-%EC%8B%A4%ED%96%89%ED%95%98%EA%B3%A0-%EA%B2%B0%EA%B3%BC-%EA%B0%80%EC%A0%B8%EC%98%A4%EA%B8%B0
                ProcessStartInfo start = new ProcessStartInfo();

                //start.WorkingDirectory = Application.StartupPath + @"\exefix"; // 프로그램 실행폴더의 하위 폴더인 exefix 폴더 지정
                start.FileName = Application.StartupPath + @"\exefix"+@"\editbin.exe";// @"ping.exe";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                start.CreateNoWindow = true;
                start.Arguments = row.Cells["Folder"].Value + @"\" + row.Cells["FileName"].Value + " /SUBSYSTEM:WINDOWS,5.01";
                textBox2.Text += "[" + row.Cells["No"].Value +  "]Start-----------------------------------------------------" + "\r\n";
                textBox2.Text +=  ":" + start.FileName + " " + start.Arguments + "\r\n";
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        textBox2.Text += reader.ReadToEnd();
                    }
                    process.WaitForExit();
                }
                textBox2.Text += "[" + row.Cells["No"].Value + "]End-----------------------------------------------------" + "\r\n";

                ////textBox2.Text += row.Cells["No"].Value + ": " + exefixFolder + @"\editbin.exe " + row.Cells["Folder"].Value + @"\" + row.Cells["FileName"].Value + "  /SUBSYSTEM:WINDOWS,5.01" + "\n";
            }
            //label2.Text = Application.StartupPath + @"\tmp1"; // 프로그램 실행폴더의 하위 폴더인 tmp1 폴더 지정
            DataTable dt_filelistinfo = GetFileListFromFolderPath(label2.Text);
            ShowDataFromDataTableToDataGridView(dt_filelistinfo, dataGridView2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (label2.Text.Equals("없음"))
            {
                return;// 은 함수 자체를 탈출
            }

            string fileFullPath = "";
            //3 날짜 시간 변경된게 있으면 복원하기
            //참고 : http://egloos.zum.com/nstyle/v/1638307
            for (int iRow=0; iRow<dataGridView1.RowCount; iRow++)
            {
                //dt1.Columns.Add("CreationTime", typeof(DateTime)); //생성 일자
                //dt1.Columns.Add("LastWriteTime", typeof(DateTime)); //마지막 수정 일자
                //dt1.Columns.Add("LastAccessTime", typeof(DateTime)); //마지막 접근 일자
                //row.Cells["Folder"].Value + @"\" + row.Cells["FileName"].Value
                fileFullPath = dataGridView1.Rows[iRow].Cells["Folder"].Value + @"\" + dataGridView1.Rows[iRow].Cells["FileName"].Value;

                if (dataGridView1.Rows[iRow].Cells["CreationTime"].Value!= dataGridView2.Rows[iRow].Cells["CreationTime"].Value)
                {
                    //Use of DateTime.Parse()   
                    DateTime dtModified = DateTime.Parse(dataGridView1.Rows[iRow].Cells["CreationTime"].Value.ToString());
                    //Change the file created time.
                    File.SetCreationTime(fileFullPath, dtModified);
                }

                if (dataGridView1.Rows[iRow].Cells["LastWriteTime"].Value != dataGridView2.Rows[iRow].Cells["LastWriteTime"].Value)
                {
                    //Use of DateTime.Parse()   
                    DateTime dtModified = DateTime.Parse(dataGridView1.Rows[iRow].Cells["LastWriteTime"].Value.ToString());
                    //Change the file modified time.
                    File.SetLastWriteTime(fileFullPath, dtModified);
                }

                if (dataGridView1.Rows[iRow].Cells["LastAccessTime"].Value != dataGridView2.Rows[iRow].Cells["LastAccessTime"].Value)
                {
                    //Use of DateTime.Parse()   
                    DateTime dtModified = DateTime.Parse(dataGridView1.Rows[iRow].Cells["LastAccessTime"].Value.ToString());
                    //Change the file LastAccessTime.
                    File.SetLastAccessTime(fileFullPath, dtModified);
                }

            }

            //label2.Text = Application.StartupPath + @"\tmp1"; // 프로그램 실행폴더의 하위 폴더인 tmp1 폴더 지정
            DataTable dt_filelistinfo = GetFileListFromFolderPath(label2.Text);
            ShowDataFromDataTableToDataGridView(dt_filelistinfo, dataGridView3);
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            if(textBox2.Text!="" && dataGridView3.Rows.Count==0 )
            {
                //3단계만 진행안된 상태에서는 초기화가 안됩니다.
                MessageBox.Show("3단계만 진행안된 상태에서는 초기화가 안됩니다.!", "경고!!!");
                return; // 은 함수 자체를 탈출
            }
            
            
            //초기화 버튼
            label2.Text = "없음";

            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            textBox2.Text = "";

            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            dataGridView3.Columns.Clear();
            dataGridView3.Rows.Clear();
            dataGridView3.Refresh();

        }
    }
}
