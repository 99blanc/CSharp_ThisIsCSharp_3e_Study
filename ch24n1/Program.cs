using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace GCHandsonAnalysis
{
    // 표준 Dispose 패턴을 구현한 고도화된 리소스 관리 클래스
    public class DetailedResourceHandler : IDisposable
    {
        // [관리되는 리소스] GC가 직접 관리하지만, 용량이 크면 명시적 해제가 도움됨
        private byte[] _managedData = new byte[1024 * 1024 * 10]; // 10MB 할당

        // [관리되지 않는 리소스] OS로부터 빌려온 자원(파일 핸들, 소켓, 메모리 등)
        // SafeHandle은 IntPtr보다 안전하며, 핸들 재활용 공격을 방어함
        private SafeWaitHandle _unmanagedHandle = new SafeWaitHandle(IntPtr.Zero, true);

        private bool _disposed = false; // 중복 해제 방지 플래그

        public DetailedResourceHandler()
        {
            Console.WriteLine(">> [객체 생성] 10MB 메모리와 비관리 핸들 할당");
        }

        // 소비자(User)가 직접 호출하는 메서드
        public void Dispose()
        {
            Dispose(true);
            // 중요: 이미 해제되었으므로 GC가 소멸자(~Finalizer)를 호출하지 않도록 한다. (성능 최적화)
            GC.SuppressFinalize(this);
            Console.WriteLine(">> [Dispose 호출] GC의 Finalize 대기열에서 제외");
        }

        // 실제 해제 로직이 들어가는 가상 메소드(상속 가능)
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // [관리되는 리소스 해제]
                _managedData = null;
                Console.WriteLine("   - 관리되는 리소스(byte[]) 참조 제거");
            }

            // [관리되지 않는 리소스 해제]
            if (_unmanagedHandle != null && !_unmanagedHandle.IsInvalid)
            {
                _unmanagedHandle.Dispose();
                Console.WriteLine("   - 관리되지 않는 핸들(SafeHandle) 해제");
            }

            _disposed = true;
        }

        // 소멸자(Finalizer): 개발자가 Dispose를 깜빡했을 때 GC가 마지막으로 호출한다.
        ~DetailedResourceHandler()
        {
            Console.WriteLine(">> [소멸자 실행] 개발자가 Dispose를 호출하지 않아 GC가 직접 정리 중...");
            Dispose(false);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== C# 가비지 컬렉터 상세 모니터링 시작 ===\n");

            // 메모리 사용량 체크
            PrintGCMemoryInfo("초기 상태");

            // 객체 생성 및 세대(Generation) 확인
            DetailedResourceHandler handler = new DetailedResourceHandler();
            Console.WriteLine($">> 객체 위치: {GC.GetGeneration(handler)}세대");
            PrintGCMemoryInfo("객체 할당 후");

            // 강제 세대 격상 시뮬레이션(GC.Collect 호출)
            Console.WriteLine("\n--- 강제 GC 수행 (객체 생존 시) ---");
            GC.Collect(); // 0세대 수집 -> handler는 살아있으므로 1세대로 승격
            GC.WaitForPendingFinalizers(); // 소멸자 작업 완료 대기
            Console.WriteLine($">> 객체 위치: {GC.GetGeneration(handler)}세대 (생존하여 승격된다.)");

            // 참조 해제 및 가비지 수집
            Console.WriteLine("\n--- 참조 해제 후 GC 수행 ---");
            handler.Dispose(); // 명시적 해제
            handler = null;    // 참조 제거

            GC.Collect(2, GCCollectionMode.Forced); // 2세대까지 강제 수집
            GC.WaitForPendingFinalizers();

            PrintGCMemoryInfo("최종 정리 후");

            Console.WriteLine("\n=== 모니터링 종료 ===");
        }

        static void PrintGCMemoryInfo(string tag)
        {
            // .NET 5+에서 지원하는 상세 메모리 정보 추출
            GCMemoryInfo info = GC.GetGCMemoryInfo();
            long totalMemory = GC.GetTotalMemory(false);

            Console.WriteLine($"[{tag}]");
            Console.WriteLine($"   사용 중인 메모리: {totalMemory / 1024 / 1024} MB");
            Console.WriteLine($"   힙 크기: {info.HeapSizeBytes / 1024 / 1024} MB");
            Console.WriteLine($"   0세대 수집 횟수: {GC.CollectionCount(0)}");
        }
    }
}