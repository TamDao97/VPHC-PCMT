import { Component, OnDestroy, AfterViewInit, forwardRef, NgZone, Inject, Output, EventEmitter, Input, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { NtsTinymceDefaultOptions } from './nts-tinymce.default';
import { NtsTinymceOptions } from './nts-tinymce.config.interface';

import 'src/assets/tinymce/tinymce.min';
declare var tinymce: any;

import 'src/assets/tinymce/themes/silver/theme.min.js';
import 'src/assets/tinymce/icons/default/icons.min.js';
import 'src/assets/tinymce/plugins/link/plugin.min.js';
import 'src/assets/tinymce/plugins/paste/plugin.min.js';
import 'src/assets/tinymce/plugins/table/plugin.min.js';
import 'src/assets/tinymce/plugins/advlist/plugin.min.js';
import 'src/assets/tinymce/plugins/autoresize/plugin.min.js';
import 'src/assets/tinymce/plugins/lists/plugin.min.js';
import 'src/assets/tinymce/plugins/code/plugin.min.js';
import 'src/assets/tinymce/plugins/visualblocks/plugin.min.js';
import 'src/assets/tinymce/plugins/print/plugin.min.js';
import 'src/assets/tinymce/plugins/preview/plugin.min.js';
import 'src/assets/tinymce/plugins/directionality/plugin.min.js';
import 'src/assets/tinymce/plugins/media/plugin.min.js';
import 'src/assets/tinymce/plugins/codesample/plugin.min.js';
import 'src/assets/tinymce/plugins/charmap/plugin.min.js';
import 'src/assets/tinymce/plugins/nonbreaking/plugin.min.js';
import 'src/assets/tinymce/plugins/anchor/plugin.min.js';
import 'src/assets/tinymce/plugins/image/plugin.min.js';
import 'src/assets/tinymce/plugins/hr/plugin.min.js';
import 'src/assets/tinymce/plugins/toc/plugin.min.js';
import 'src/assets/tinymce/plugins/pagebreak/plugin.min.js';
import 'src/assets/tinymce/plugins/insertdatetime/plugin.min.js';
import 'src/assets/tinymce/plugins/textcolor/plugin.min.js';
import 'src/assets/tinymce/plugins/wordcount/plugin.min.js';
import 'src/assets/tinymce/plugins/imagetools/plugin.min.js';
import 'src/assets/tinymce/plugins/contextmenu/plugin.min.js';
import 'src/assets/tinymce/plugins/textpattern/plugin.min.js';
import 'src/assets/tinymce/plugins/searchreplace/plugin.min.js';
import 'src/assets/tinymce/plugins/fullscreen/plugin.min.js';
//import 'src/assets/tinymce/skins/content/default/content.min.css';

const noop = () => {
};

@Component({
	selector: 'app-nts-tinymce',
	template: '<div id="{{elementId}}"></div>',
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => NtsTinymceComponent),
			multi: true
		}
	]
})
export class NtsTinymceComponent implements ControlValueAccessor, AfterViewInit, OnDestroy, OnInit {

	 array = new Uint32Array(1);
	public elementId: string = 'tiny-' + window.crypto.getRandomValues(this.array).toString().substring(2);
	//Math.random().toString(36).substring(2);
	public editor: any;
	// eslint-disable-next-line @angular-eslint/no-output-native
	@Output() focus = new EventEmitter();
	@Input() config: any;
	@Input() isCenter: boolean;
	private onTouchedCallback: () => void = noop;
	private onChangeCallback: (_: any) => void = noop;
	private innerValue: string;

	private options: any;
	constructor(
		private zone: NgZone,

	) {

	}

	ngOnInit() {
		this.options = Object.assign(new NtsTinymceDefaultOptions(), this.config);
		const iscenter = this.isCenter;
		this.options.selector = '#' + this.elementId;
		this.options.setup = (editor:any) => {
			this.editor = editor;
			editor.on('change keyup', () => {
				const content = editor.getContent();
				this.value = content;
			});
			editor.on('focus', (e:any) => {
				this.focus.emit(e);
			});
			editor.on('ResizeEditor', function (e:any) {

				// do whatever you need here
			});
			editor.on('init', (e:any) => {
				if (iscenter === true) {
					const content = editor.getContent();
					if (!content.includes("text-align: center")) {
						setTimeout(function () {
							editor.execCommand('JustifyCenter');
						}, 1);
					}
				}
			});

			if (typeof this.config.setup === 'function') {
				this.config.setup(editor);
			}
		}
		this.options.init_instance_callback = (editor:any) => {
			editor && this.value && editor.setContent(this.value)
			if (typeof this.config.init_instance_callback === 'function') {
				this.config.init_instance_callback(editor);
			}
		}
		if (this.config.auto_focus) {
			this.options.auto_focus = this.elementId;
		}
	}


	ngAfterViewInit() {
		if (this.options.baseURL) {
			tinymce.baseURL = this.options.baseURL;
		}
		tinymce.init(this.options);
	}

	ngOnDestroy() {
		tinymce.remove(this.editor);
	}

	// get accessor
	get value(): any {
		return this.innerValue;
	};

	// set accessor including call the onchange callback
	set value(v: any) {
		if (v !== this.innerValue) {
			this.innerValue = v;
			this.zone.run(() => {
				this.onChangeCallback(v);
			});

		}
	}
	// From ControlValueAccessor interface
	writeValue(value: any) {
		if (value !== this.innerValue) {
			this.innerValue = value;
			if (!value) {
				value = '';
			}
			this.editor && this.editor.setContent(value);
		}
	}

	registerOnChange(fn: any) {
		this.onChangeCallback = fn;
	}

	registerOnTouched(fn: any) {
		this.onTouchedCallback = fn;
	}
}
