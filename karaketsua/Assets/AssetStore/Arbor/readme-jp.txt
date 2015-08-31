-----------------------------------------------------
            Arbor: State Diagram Editor
          Copyright (c) 2014 Cait Sith Ware
          http://caitsithware.com/wordpress/
          support@caitsithware.com
-----------------------------------------------------

Arborを購入いただきありがとうございます！

【更新方法】

1. すでにあるArborフォルダを削除
2. Arborをインポート

【主な流れ】

1. GameObjectにArborFSMをアタッチ。
2. ArborFSMのインスペクタにあるOpen Editorボタンをクリック
3. Arbor EditorでStateを作成。
4. StateにBehaviourをアタッチ。
5. BehaviourからStateへの遷移を接続。

【JavascriptやBooを使用する場合】

1. Assets以下にPluginsフォルダがない場合、Pluginsフォルダを作成
2. Pluginsフォルダの中にArborフォルダを作成
3. Arbor/Scripts/にあるCoreフォルダをそのフォルダの中に移動

【サンプルシーン】

サンプルシーンは以下にあります。
Assets/Arbor/Examples/Scenes/

【ドキュメント】

詳しいドキュメントはこちらをご覧ください。
http://arbor.caitsithware.com/

【サポート】

フォーラム : http://forum-arbor.caitsithware.com/

メール : support@caitsithware.com

【更新履歴】

Ver 1.7.4:
- Add : Agent系Behaviour追加。
- Add : uGUI系Behaviour追加。
- Add : uGUI系Tween追加。
- Add : SendEventGameObject追加。
- Add : SendMessageGameObjectに値渡し機能追加。
- Fix : AnimatorParameterReferenceの参照先がAnimatorControllerを参照していなかったときにエラーが出るのを修正。
- Other : uGUI対応に伴いUnity最低動作バージョンを4.6.7f1に引き上げ。

Ver 1.7.3:
- Add : OnMouse系Transition追加
- Fix : 選択ステートへの移動時のスクロール位置修正
- Other : ステートリストを名前順でソートするように変更。
- Other : Arbor Editorの左上方向へも無限にステートを配置できるように変更。
- Other : マニュアルサイトを一新。

Ver 1.7.2:
- Add : ArborEditorにコメントノードを追加。
- Add : 挙動追加時に検索できるように対応。
- Add : CalcAnimatorParameter追加。
- Add : AnimatorStateTransition追加。
- Add : 遷移線を右クリックで遷移元と遷移先へ移動できるように追加。
- Fix : Prefab元に挙動追加するとPrefab先に正しく追加されないのを修正。
- Other : ForceTransitionをGoToTransitionに改名。
- Other : 挙動追加で表示される組み込みBehaviourの名前を省略しないように変更。
- Other : 組み込みBehaviourをAdd Componentに表示しないように変更。

Ver 1.7.1:
- Add : ステートリストを追加。
- Add : ParamneterReferenceのPropertyDrawerを追加。
- Add : 要素の削除ができるリスト用のGUI、ListGUIを追加。
- Fix : CalcParameterのboolValueがintになっていたのを修正。

Ver 1.7.0;
- Add : パラメータコンテナ。
- Fix : OnStateBegin()で状態遷移した場合、それより下のBehaviourを実行しないように修正。

Ver 1.6.3f1:
- Unity5 RC3でエラーが出るのを修正。
- Unity5 RC3対応により、OnStateEnter/OnStateExitをOnStateBegin/OnStateEndに改名。

Ver 1.6.3:
- Transitionにforceフラグを追加。trueにすると呼び出し時にその場で遷移するようにできる。
- ソースコードへドキュメントコメント埋め込み。
  Player Settings の Scripting Define Symbols に ARBOR_DOC_JA を追加すると日本語でドキュメントコメントが見れるようになります。
- スクリプトリファレンスをAssets/Arbor/Docsに配置。
  解凍してindex.htmlを開いてください。

Ver 1.6.2:
- FIX : OnStateEnterでステート遷移できないのを修正。

Ver 1.6.1:
- FIX : Mac環境でGridボタン押すとエラーが表示される。

Ver 1.6:
- ADD : 常駐ステート。
- ADD : 多言語対応。
- ADD : ArborFSMに名前を付けられるように対応。
- FIX : グリッドサイズを変更してもスナップ間隔に反映されない。
- FIX : ArborFSMのコンポーネントをコピー＆ペーストした際にStateBehaviourが消失する問題の対処。
- FIX : SendTriggerを現在有効なステートにのみ送るように変更。
- FIX : ArborFSMを無効にしてもStateBehaviourが動き続ける。

Ver 1.5:
- ADD : ステートの複数選択に対応。
- ADD : ショートカットキーに対応。
- ADD : グリッド表示対応。
- FIX : Behaviour追加時にデフォルトで広げた状態にする。
- FIX : StateLinkのドラッグ中にステートへのマウスオーバーがずれて反応する。
 
ver 1.4:
- ADD : Tween系Behaviour追加。
 - Tween / Color
 - Tween / Position
 - Tween / Rotation
 - Tween / Scale
- ADD : Add Behaviourに表示されないようにするHideBehaviour属性追加。
- ADD : Behaviourのヘルプボタンから組み込みBehaviourのオンラインヘルプ表示。

ver 1.3:
- ADD : 組み込みBehaviour追加。
 - Audio / PlaySoundAtPoint
 - GameObject / SendMessage
 - Scene / LoadLevel
 - Transition / Force
- ADD : シーンを跨いだコピー&ペースト。
- FIX : Stateをコピーしたあとシーンを保存するとメモリリークの警告が表示される。
- FIX : StateLinkの接続ドラッグ中に画面スクロールすると矢印が残る。

ver 1.2:
- ADD : StateBehaviourの有効チェックボックス。
- FIX : Arbor Editorの最大化を解除するとエラーが出る。
- FIX : 生成したC#スクリプトを編集すると改行コードの警告が出る。

ver 1.1:
- ADD : JavaScriptとBooのスクリプト生成。
- ADD : Stateのコピー＆ペースト。
- ADD : StateBehaviourのコピー＆ペースト。
- FIX : スクリプトがMissingになったときの対応。
- FIX : StateLinkの配列が表示されないのを修正。

ver 1.0.1:
- FIX : Unity4.5でのエラー。
- FIX : エディタ上での実行時にArbor Editorが再描画されない。
- FIX : ArborFSMのInspector拡張のクラス名。
