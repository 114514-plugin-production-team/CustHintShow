using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;

namespace CustHintShow
{
    public class Display
    {
        private static readonly Dictionary<Player, Display> PlayerDisplays = new Dictionary<Player, Display>();
        public static Display Get(Player player)
        {
            if (!PlayerDisplays.TryGetValue(player, out var display))
            {
                display = new Display(player);
                PlayerDisplays[player] = display;
            }
            return display;
        }
        private readonly Player _player;
        private readonly List<Hints.Hint> _hints = new List<Hints.Hint>();
        private GameObject _canvasObject;
        private List<GameObject> _textObjects = new List<GameObject>();
        private Display(Player player)
        {
            _player = player;
            InitializeUI();
        }

        private void InitializeUI()
        {
            // 创建 Canvas
            _canvasObject = new GameObject("HintCanvas");
            var canvas = _canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999; // 确保 UI 显示在最上层

            // 添加 CanvasScaler 以适应不同分辨率
            var canvasScaler = _canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);

            // 添加 GraphicRaycaster 以支持 UI 交互
            _canvasObject.AddComponent<GraphicRaycaster>();
        }
        public void Add(Hints.Hint hint)
        {
            _hints.Add(hint);
            UpdatePlayerHints();
        }

        public void Delete(Hints.Hint hint)
        {
            _hints.Remove(hint);
            UpdatePlayerHints();
        }

        private void UpdatePlayerHints()
        {
            // 清除旧的提示
            foreach (var textObject in _textObjects)
            {
                UnityEngine.Object.Destroy(textObject);
            }
            _textObjects.Clear();

            // 创建新的提示
            foreach (var hint in _hints)
            {
                var textObject = new GameObject("HintText");
                textObject.transform.SetParent(_canvasObject.transform);

                var textMeshPro = textObject.AddComponent<TextMeshProUGUI>();
                textMeshPro.text = hint.Text;
                textMeshPro.fontSize = hint.Size;
                textMeshPro.color = Color.white;
                textMeshPro.alignment = TextAlignmentOptions.Center;

                // 设置提示位置
                var rectTransform = textObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(hint.X, hint.Y);

                _textObjects.Add(textObject);
            }

        }
        
    }
    public static class PlayerExtensions
    {
        /// <summary>
        /// 为玩家添加自定义提示
        /// </summary>
        /// <param name="player">玩家对象</param>
        /// <param name="x">X 坐标</param>
        /// <param name="y">Y 坐标</param>
        /// <param name="size">字体大小</param>
        /// <param name="text">提示内容</param>
        /// <param name="duration">显示时间（秒）</param>
        public static void AddHint(this Player player, float x, float y, int size, string text, float duration)
        {
            // 获取或添加 PlayerHintController 组件
            var hintController = player.GameObject.GetComponent<PlayerHintController>();
            if (hintController == null)
            {
                hintController = player.GameObject.AddComponent<PlayerHintController>();
                hintController.Player = player;
            }

            // 添加提示
            hintController.AddHint(x, y, size, text, duration);
        }
    }

    public class PlayerHintController : MonoBehaviour
    {
        public Player Player { get; set; }
        private GameObject _canvasObject;
        private readonly List<GameObject> _textObjects = new List<GameObject>();

        private void Start()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // 创建 Canvas
            _canvasObject = new GameObject("HintCanvas");
            var canvas = _canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999; // 确保 UI 显示在最上层

            // 添加 CanvasScaler 以适应不同分辨率
            var canvasScaler = _canvasObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);

            // 添加 GraphicRaycaster 以支持 UI 交互
            _canvasObject.AddComponent<GraphicRaycaster>();
        }

        public void AddHint(float x, float y, int size, string text, float duration)
        {
            // 创建提示文本对象
            var textObject = new GameObject("HintText");
            textObject.transform.SetParent(_canvasObject.transform);

            // 添加 TextMeshProUGUI 组件
            var textMeshPro = textObject.AddComponent<TextMeshProUGUI>();
            textMeshPro.text = text;
            textMeshPro.fontSize = size;
            textMeshPro.color = Color.white;
            textMeshPro.alignment = TextAlignmentOptions.Center;

            // 设置提示位置
            var rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(x, y);

            // 添加到提示列表
            _textObjects.Add(textObject);

            // 设置定时器，在指定时间后删除提示
            StartCoroutine(RemoveHintAfterDuration(textObject, duration));
        }

        private IEnumerator RemoveHintAfterDuration(GameObject textObject, float duration)
        {
            yield return new WaitForSeconds(duration);
            _textObjects.Remove(textObject);
            Destroy(textObject);
        }
    }
}
