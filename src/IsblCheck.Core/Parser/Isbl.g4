grammar Isbl;

@parser::header {#pragma warning disable 3021}
@lexer::header {#pragma warning disable 3021}

/*******************************************************************************
*********************************Правила лексера********************************
*******************************************************************************/
// Английские регистронезависимые символы.
fragment A:('a'|'A');
fragment B:('b'|'B');
fragment C:('c'|'C');
fragment D:('d'|'D');
fragment E:('e'|'E');
fragment F:('f'|'F');
fragment G:('g'|'G');
fragment H:('h'|'H');
fragment I:('i'|'I');
fragment J:('j'|'J');
fragment K:('k'|'K');
fragment L:('l'|'L');
fragment M:('m'|'M');
fragment N:('n'|'N');
fragment O:('o'|'O');
fragment P:('p'|'P');
fragment Q:('q'|'Q');
fragment R:('r'|'R');
fragment S:('s'|'S');
fragment T:('t'|'T');
fragment U:('u'|'U');
fragment V:('v'|'V');
fragment W:('w'|'W');
fragment X:('x'|'X');
fragment Y:('y'|'Y');
fragment Z:('z'|'Z');

// Русские регистронезависимые символы.
fragment А:('а'|'А');
fragment Б:('б'|'Б');
fragment В:('в'|'В');
fragment Г:('г'|'Г');
fragment Д:('д'|'Д');
fragment Е:('е'|'Е');
fragment Ё:('ё'|'Ё');
fragment Ж:('ж'|'Ж');
fragment З:('з'|'З');
fragment И:('и'|'И');
fragment Й:('й'|'Й');
fragment К:('к'|'К');
fragment Л:('л'|'Л');
fragment М:('м'|'М');
fragment Н:('н'|'Н');
fragment О:('о'|'О');
fragment П:('п'|'П');
fragment Р:('р'|'Р');
fragment С:('с'|'С');
fragment Т:('т'|'Т');
fragment У:('у'|'У');
fragment Ф:('ф'|'Ф');
fragment Х:('х'|'Х');
fragment Ц:('ц'|'Ц');
fragment Ч:('ч'|'Ч');
fragment Ш:('ш'|'Ш');
fragment Щ:('щ'|'Щ');
fragment Ъ:('ъ'|'Ъ');
fragment Ы:('ы'|'Ы');
fragment Ь:('ь'|'Ь');
fragment Э:('э'|'Э');
fragment Ю:('ю'|'Ю');
fragment Я:('я'|'Я');

// Разделитель.
WS: [ \t\r\n]+->skip;

// Комментарии.
BLOCK_COMMENT: '/*' .*? '*/'->skip;
LINE_COMMENT: '//' ~[\r\n]*->skip;

// Ключевые слова.
AND: A N D | И;
ELSE: E L S E | И Н А Ч Е;
ENDEXCEPT: E N D E X C E P T;
ENDFINALLY: E N D F I N A L L Y;
ENDFOREACH: E N D F O R E A C H | К О Н Е Ц В С Е;
ENDIF: E N D I F | К О Н Е Ц Е С Л И;
ENDWHILE: E N D W H I L E | К О Н Е Ц П О К А;
EXCEPT: E X C E P T;
EXITFOR: E X I T F O R;
FINALLY: F I N A L L Y;
FOREACH: F O R E A C H | В С Е;
IF: I F | Е С Л И;
IN: I N | В;
NOT: N O T | Н Е;
OR: O R | И Л И;
TRY: T R Y;
WHILE: W H I L E | П О К А;

// Операторы.
EQ: '=';
NEQ: '<>';
GT: '>';
GEQ: '>=';
LT: '<';
LEQ: '<=';
STR_EQ: '==';
STR_NEQ: '<<>>';
STR_GT: '>>';
STR_GEQ: '>>=';
STR_LT: '<<';
STR_LEQ: '<<=';
PLUS: '+';
MINUS: '-';
MUL: '*';
DIV: '/';
POINTER:'^';
CONCAT: '&';
L_PAREN: '(';
R_PAREN: ')';
L_BRACKET: '[';
R_BRACKET: ']';
COLON : ':';
COMMA: ';';
DOT: '.';

// Литералы.
STRINGLITERAL: '"' ~["]* '"' | '\'' ~[\']* '\'';
NUMBERLITERAL: [0-9]+ ([.] [0-9]+ )?;
IDENTLITERAL : ('a'..'z' | 'A'..'Z' | 'а'..'я' | 'ё' | 'А'..'Я' | 'Ё' | '!') ('a'..'z' | 'A'..'Z' | 'а'..'я' | 'ё' | 'А'..'Я' | 'Ё' | '0'..'9'| '_')*;

/*******************************************************************************
*********************************Правила парсера********************************
*******************************************************************************/
// Блок предложений.
statementBlock
    : ( declareVariableStatement 
      | assignStatement
      | invokeStatement
      | ifStatement
      | foreachStatement
      | whileStatement
      | tryStatement 
      | exitforStatement
      )*
    ;

// Объявление переменной.
declareVariableStatement
    : variable COLON typedefinition
    ;
// Означивание.
assignStatement
    : ( variable COLON typedefinition
      | variable indexer? (DOT invocationCall)*
      | function (DOT invocationCall)+
      ) EQ expression
    ;
// Вызов метода.
invokeStatement
    : variable indexer? (DOT invocationCall)+
    | function (DOT invocationCall)*
    ;
// Условие.
ifStatement
    : IF expression statementBlock (ELSE statementBlock)? ENDIF
    ;
// Совместный цикл.
foreachStatement
    : FOREACH variable (IN | EQ) expression statementBlock ENDFOREACH
    ;
// Цикл с предусловием.
whileStatement
    : WHILE expression statementBlock ENDWHILE
    ;
// Обработка исключений.
tryStatement
    : TRY statementBlock ( EXCEPT statementBlock ENDEXCEPT 
                         | FINALLY statementBlock ENDFINALLY
                         )
    ;
// Выход из цикла.
exitforStatement
    : EXITFOR
    ;
// Выражение.
expression
    : operand
    | (PLUS | MINUS) expression
    | NOT expression
    | expression (MUL | DIV) expression
    | expression (PLUS | MINUS | CONCAT) expression
    | expression (EQ | NEQ | GT | GEQ | LT | LEQ | STR_EQ | STR_NEQ | STR_GT | STR_GEQ | STR_LT | STR_LEQ) expression
    | expression AND expression
    | expression OR expression
    ;
// Операнд.
operand
    : unsignedNumber
    | string
    | variable indexer? (DOT invocationCall)*
    | variable POINTER
    | function (DOT invocationCall)*
    | L_PAREN expression R_PAREN;
// Функция.
function
    : identifier L_PAREN parameterList R_PAREN
    ;
// COM вызов.
invocationCall
    : identifier (L_PAREN parameterList R_PAREN)?
    ;
// Индексатор.
indexer
    : L_BRACKET parameterList R_BRACKET
    ;
// Типизатор.
typedefinition
    : type (DOT subtype)?
    ;
// Список параметров.
parameterList
    : expression? (COMMA expression?)*
    ;
// Идентификатор. Есть функция IN, включаем её, для учета.
identifier
    : IDENTLITERAL
    | IN
    ;
// Переменная.
variable
    : IDENTLITERAL
    ;
// Тип.
type
    : IDENTLITERAL
    ;
// Суб-тип.
subtype
    : IDENTLITERAL
    ;
// Безнаковое число.
unsignedNumber
    : NUMBERLITERAL
    ;
// Строка.
string
    : STRINGLITERAL
    ;